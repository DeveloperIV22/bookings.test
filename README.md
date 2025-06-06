# Introduction

The project is in the style of domain driven design but I did not bother to make Dependency injection or a separate project for interfaces, as the requirements stated there is no need for complex frameworks.
And the algorithm is made by me alone, without any help from ChatGPT.

The algorithm is made to be simple to understand, NOT efficient at this point in time.
I understand the algorithm is not perfect, but I did not want to steal something from the internet.

I worked about 10-13 hours on this project.

I also have experience with optaplanner, so I can use it to setup some booking optimization problems if that is in any way required.
Perhaps I can do a small home assigned with it.
https://www.optaplanner.org/

# Date Handling

The start and end date are 'INCLUSIVE'.

if user has 2024-09-01 and 2024-09-04, then that means he stayed in hotel 4 days and left the 5th day - 5th september, and somebody can book on 5th september.

# usage:

./ConsoleApp.exe --hotels hotels.json --bookings bookings.json

or just open the solution in visual studio and hit 'run' and this line will be hit
RunBookingConsoleLoop(new HotelAndBookingOptions() { BookingsFile = "bookings.json", HotelsFile = "hotels.json" });

The program should implement 2 commands described below.
The program should exit when a blank line is entered.

Availability Command

Example console input:
Availability(H1, 20240901, SGL)
Availability(H1, 20240901-20240903, DBL)

Output: the program should give the availability as a number for a room type that date range. Note: hotels sometimes accept overbookings so the value can be negative to indicate this.

RoomTypes Command

Example console input:  
RoomTypes(H1, 20240904, 3)  
RoomTypes(H1, 20240905-20240907, 5)

Output: The program should return a hotel name, and a comma separated list of room type codes needed to allocate number of people specified in the specified time. Minimise the number of rooms to accommodate the required number of people. Avoid partially filling rooms. If a room is partially filled, the room should be marked with a "!”.
Show an error message if allocation is not possible.
