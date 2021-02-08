# AirTicketingSystem

# Drobne uwagi

Rozróżniam techniczne utworzenie instancji klasy od domenowego "utworzenia" agregatu.
Np.
var booking = new Booking();
nie oznacza istniejącego w rozuminiu domeny bookingu. Dopiero zapis:
booking.Start(...);
oznacza, że booking zaczyna egzystować w rozumieniu domeny.
Pierwsza inicjalizacja (poprzez konstruktor) jest jedynie inicjalizacją techniczną. Służy wstrzyknięciu zależności do klasy.
Druga inicjalizacja (poprzez metodę Start, Initialize, Schedule, Create itp.) jest inicjalizacją logiczną, zaistnieniem bytu w domenie.
W związku z tym w command handlerach mogą pojawić się zapisy:
var booking = _repository.Get(command.BookingId);
i taki zapis nie powoduje wyjątku nawet jeżeli bookingu jeszcze nie ma. W takiej sytuacji repozytorium zwróci pusty, niezainicjalizowany w rozumeniu domeny obiekt - bez przypisanego id.
Dopiero wywołanie metody Start ten obiekt inicjalizuje.
Dzięki takiemu podejściu unifikuje się podejście do testów bo zarówno agregaty zainicjalizowane jak i niezainicjalizowane traktujemy w testach tak samo.

Puste (nie zawierające ani jednej linijki kodu) metody Apply są jak najbardziej okej. W przypadku event sourcingu wystąpienie danego eventu zawsze warunkowane jakąś logiką domenową. Natomiast nie zawsze wystąpienie eventu wpływa na dalszą logikę domenową. Jeżli nie wpływa to nie ma sensu w metodzie Apply zapisywać nic do prywatnych pól.
Przykładem może być wystąpienie eventu NoteAdded. Jeżli od dodania notatki nic dalej nie zależy to wystarczający jest sam fakt występienia tego eventu. Nie będziemy zmieniać stanu pól w agregacie bo na nic to nie wpływa.

Wiadomości w wyjątkach domenowych DomainLogicException są obecnie zhardkodowane po angielsku. W rozwiązaniu docelowym było by to oczywiście miejsce na lokalizację i wykorzystanie resource'ów językowych.

W messageach nie powinny być używane value objecty ze względu na to, że message powinny być immutable, oraz powinny być transportowalne co oznacza, że mogą pochodzić z innych aplikacji napisanych w innych językach. W związku z tym zarówno commandy jak i eventy zawierają:
- Guid zamiast FlightInstanceId,
- Guid zamiast CustomerId
- decimal zamiast Money
- itd.

W przypadku lotnisk lotnisko nie jest agregatem (w rozubodwanym systemie zapewne by było). Agregatem jest baza lotnisk Airports. Taka baza w cały systemie jest tylko jedna w związku z tym w odróżnieniu od pozostałych agregatów istnieje globalne id bazy lotnisk (klasa GlobalAirportsId).

Commandy i eventy nie zawierają jakichkolwiek walidacji argumentów w swoich konstruktorach.
Jest tak ponieważ:
- commandy to obiekty transportowalne co oznacza, że mogą pochodzić z innych aplikacji napisanych w innych językach
- eventy są obiektami domenowymi produkowanymi przez domenę (agregaty / serwisy domenowe), a więc z założenia są inicjalizowane poprawnie i umieszczanie nich jakiejkolwiek logiki było by dublowaniem logiki.

W przypadku agregatu FlightAggregate jego identyfikatorem (kluczem unikalnym) jest klasa FlightUid - zawierająca suffix Uid w odróżnieniu do pozostałych agregatów gdzie nazwa klasy identyfikatora kończy się suffixem Id.
Są ku temu dwa powody:
- określenie FlightId (suffix Id) pochodzi z języka konretnej domeny - występuje w opisie zadania jako połączenie oznaczenia linii lotniczych, numeru, i trzech liter w związku z tym nie mogłem użyć tej nazyw dla klucza uniklanego
- klucze naturalne są problematyczne, potrafią okazywać się nieunikalnymi, rozszerzać się itd. w związku z tym nie mogłem zamiast klucza unikalnego wykorzystać tego klucza

Opis zadania pozostawia pytania np.:
- Co w sytuacji gdy cena bazowa instancji lotu zostanie zmieniona?
- Co w sytuacji gdy w przypadku tenanta B nie zostaną zapisane zniżki, a nastąpi zmiana ceny bazowej instancji lotu albo nastąpi przeliczenie dostępnych kryteriów zniżkowych - np. niektóre kryteria zostaną wyłączone w trakcie rezerwowania lotu?
W związku z tym implementacja zawiera wyłącznie taką ligikę domenową jaka wynika bezpośrednio z treści zadania.