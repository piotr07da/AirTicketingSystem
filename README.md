# AirTicketingSystem

W ramach zadania powstały:
- warstwa Aplikacji - oparta o Commandy (CQRS)
- warstwa Domeny - oparta o EventSourcing
- warstwa Infrastruktury - brak faktycznej implementacji, powstała implementacja na potrzeby Unit Testów
- narzędzie do unit testów systemu opartego o event sourcing + ddd + cqrs
- testy jednostkowe zgodne ze współczesnym rozumieniem jednostek jako operacji publicznego kontraktu systemu. W przypadku zastosowanego podejścia publicznym kontraktem są Commandy wpadające do systemu i wypadające z niego Eventy.

## Projekt
Analiza domeny została przeprowadzona dzięki metodzie event storming. Następnie został utworzony prosty diagram zależności obiektów domenowych.
![ATS project](ats.png)

## Agregaty

#### Booking
Booking to rezerwacja lotu reprezentująca cały jej przebieg od momentu rozpoczęcia rezerwacji do momentu zatwierdzenia lub anulowania rezerwacji. Nawet jeżeli w systemie powstał Booking, który nie ma przypisanego klienta to fakt istnienia takiego bookingu może nieść cenne informacje na temat stopnia zainteresowania danym lotem.
Operacje bookingu:
- Start - utworzenie rezerwacji dla określonej instancji lotu i jego ceny bazowej
- AssignCustomer - przypisanie klienta
- AddDiscountOffer - dodanie dostępnej oferty zniżkowej. Oferty zniżkowe dodawane są zawsze niezależnie od grupy Tenant. Dodanie oferty zniżkowej jest czymś innym niż zapisaniem zastosowanej zniżki.
- ApplyDiscountOffer - zastosowanie zniżki
- Cancel - anulowanie rezerwacji
- Confirm - zatwierdzenie rezerwacji

#### FlightInstance
FlightInstance to pojedyncza instancja zaplanowanego lotu. W odróżnieniu do lotu, który definiuje dni tygodnia i godzinę wylotu FlightInstance definiuje konkretną datę lotu oraz cenę. FlightInstance jest również odpowiedzialny za kontrolowanie liczby rezerwacji lotu (potencjalnie overbooking itd.).
- Create - utworzenie instancji lotu
- AddBooking - dodanie rezerwacji

#### Flight
Lot zgodnie z opisem w zadaniu prezentuje zaplanowany lot cykliczny
- Schedule - zaplanowanie lotu cyklicznego
- AddFlightInstance - dodanie lotu na konkretny dzień

#### Customer
Reprezentuje klienta
- Register - rejestracja klienta

#### Airports
Airports reprezentuje bazę wszystkich obsługiwanych przez system lotnisk.
- AddAirport - dodanie nowego lotniska do bazy lotnisk

## Usługi domenowe

#### BookingStartingService
Usługa obsługująca command StartBookingCommand operująca na dwóch bytach domenowych - Booking (metoda Start) oraz FlightInstance (metoda AddBooking).

#### DiscountService
Usługa obsługująca command RefreshDiscountOffersCommand operująca na jednym bycie domenowym - Booking (metoda AddDiscountOffer).

#### FlightInstanceCreationService
Usługa obsługująca command CreateFlightInstanceCommand operująca na dwóch bytach domenowych - FlightInstance (metoda Create) oraz Flight (metoda AddFlightInstance).

## Drobne uwagi

#### new Flight() vs flight.Schedule(...)
Rozróżniam techniczne utworzenie instancji klasy od domenowego "utworzenia" agregatu.
```c#
var booking = new Booking();
````
Powyższy zapis jest jedynie technicznym utworzeniem instancji klasy Booking. Nie posiada zainicjalizowanego identyfikatora ani innych właściwości bo nie do tego służy konstruktor. Konstruktor jest jedynie inicjalizatorem instancji klasy. Służy wstrzyknięciu zależności. W języku domeny nie występuje coś takiego jak konstruktor. Takich bytów jak Booking, Customer, Flight się nie konstruuje na co wskazywało by niejawnie występujące słowo konstruktor tylko startuje (Booking), rejestruje (Customer), planuje (Flight).
```c#
booking.Start(...);
```
W związku z tym w command handlerach mogą pojawić się zapisy:
```c#
var booking = _repository.Get(command.BookingId);
```
i taki zapis nie powoduje wyjątku nawet jeżeli bookingu jeszcze nie ma. W takiej sytuacji repozytorium zwróci pusty, niezainicjalizowany w rozumeniu domeny obiekt - bez przypisanego Id. Dopiero wywołanie metody Start  ten obiekt inicjalizuje.
Ponadto dzięki takiemu podejściu unifikuje się podejście do testów bo zarówno agregaty zainicjalizowane jak i niezainicjalizowane w rozumeniu domeny traktujemy w testach tak samo i tak samo powołujemy do życia.

#### void Apply() { }
Puste (nie zawierające ani jednej linijki kodu) metody Apply są jak najbardziej okej. W przypadku event sourcingu wystąpienie danego eventu zawsze warunkowane jest jakąś logiką domenową, natomiast nie zawsze wystąpienie eventu wpływa na dalszą logikę domenową. Jeżli nie wpływa to nie ma sensu w metodzie Apply zapisywać nic do prywatnych pól.
Przykładem może być wystąpienie eventu NoteAdded. Jeżli od dodania notatki nic dalej nie zależy to wystarczający jest sam fakt występienia tego eventu. Nie będziemy wprowadzać pola _note w klasie agregatu, jeżeli do tego pola nic nie zależy.

#### Tłumaczenia
Wiadomości w wyjątkach domenowych DomainLogicException są obecnie zhardkodowane po angielsku. W rozwiązaniu docelowym było by to oczywiście miejsce na lokalizację i wykorzystanie resource'ów językowych.

#### ValueObjecty w Commandach i Eventach
W messageach nie powinny być używane value objecty ze względu na to, że message powinny być immutable, oraz powinny być transportowalne co oznacza, że mogą pochodzić z innych aplikacji napisanych w innych językach, w których logika domenowa nie będzie odzwierciedlona. Poza tym ValueObjecty mogą ulegać zmianom i zawarcie ich w Commandach i Eventach uniemożliło by deserializację. W związku z tym zarówno commandy jak i eventy zawierają:
- Guid zamiast FlightInstanceId,
- Guid zamiast CustomerId
- decimal zamiast Money
- itd.

#### Brak walidacji w konstruktorach Commandów i Eventów
Commandy i eventy nie zawierają jakichkolwiek walidacji argumentów w swoich konstruktorach.
Jest tak ponieważ:
- commandy to obiekty transportowalne co oznacza, że mogą pochodzić z innych aplikacji napisanych w innych językach i logika walidacji napisana w c# i tak nie ma znaczenia
- eventy są obiektami domenowymi produkowanymi przez domenę (agregaty / serwisy domenowe), a więc z założenia są inicjalizowane poprawnie i umieszczanie nich jakiejkolwiek logiki było by dublowaniem logiki.

#### Uproszczenie lotnisk
W przypadku lotnisk lotnisko nie jest agregatem (w rozubodwanym systemie zapewne by było). Agregatem jest baza lotnisk Airports. Taka baza w całym systemie jest tylko jedna w związku z tym w odróżnieniu od pozostałych agregatów istnieje globalne id bazy lotnisk (klasa GlobalAirportsId).

#### FlightUid vs FlightId
W przypadku agregatu FlightAggregate jego identyfikatorem (kluczem unikalnym) jest klasa FlightUid - zawierająca suffix Uid w odróżnieniu do pozostałych agregatów gdzie nazwa klasy identyfikatora kończy się suffixem Id.
Są ku temu dwa powody:
- określenie FlightId (suffix Id) pochodzi z języka konretnej domeny - występuje w opisie zadania jako połączenie oznaczenia linii lotniczych, numeru, i trzech liter w związku z tym nie mogłem użyć tej nazyw dla klucza uniklanego
- klucze naturalne są problematyczne, potrafią okazywać się nieunikalnymi, rozszerzać się itd. w związku z tym nie mogłem zamiast klucza unikalnego wykorzystać tego klucza

#### Pytania bez odpowiedzi
Opis zadania pozostawia pytania bez odpowiedzi np.:
- Co w sytuacji gdy w przypadku tenanta B nie zostaną zapisane zniżki, a TicketingAgent jeszcze raz zastosuje daną niżkę? Teoretycznie może zastosować daną zniżkę 1000000 razy bo bez zapisywania zastosowanych zniżek nie da się przed tym zabezpieczyć.
- Co w sytuacji gdy cena bazowa instancji lotu zostanie zmieniona?
- Co w sytuacji gdy w przypadku tenanta B nie zostaną zapisane zniżki, a nastąpi zmiana ceny bazowej instancji lotu albo nastąpi przeliczenie dostępnych kryteriów zniżkowych - np. niektóre kryteria zostaną wyłączone w trakcie rezerwowania lotu?
W związku z tym implementacja zawiera wyłącznie taką logikę domenową jaka wynika bezpośrednio z treści zadania.