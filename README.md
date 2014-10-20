## TakeMeThere Inc. Kata

We are a new startup TakeMeThere Inc. that wants to revolutionize the business of urban transportation. We want to build an app that allows users to book taxis based on a variety of filters and user preferences. Both the customers and the taxi owners should be able to rate each other, in order to recognize an excellent service and an excellent client (Based on a scale from 1 to 5, 1 being the worst value and 5 the best).

A customer can order a taxi based on its size (Small, Medium or Large) and on a set of features available (number of seats, air conditioner, wheelchair accessible, extra baggage space, luxurious equipment...). A customer can set personal preferences such as the minimum rating a taxi needs to have (e.g. they only want taxis with a rating above 3) which will be applied every time the customer uses the application.

On the other hand, a taxi owner can set personal preferences regarding working hours (only during the day, during the night or no preferences), the trip length (based on distance, only long trip, only small trips or no preferences) or the minimum rating a customer needs to have.

The goal is to implement all the classes needed to hold these functional requirements.
Implement as well an API that exposes the following methods:

    * getTaxis: It returns a list of most promising taxis including their location and price, based on the search filtering criteria
      * Input:
        * Customer
        * Current Customer Location
        * Destination Customer Location
        * Filter
          * Nearest taxis
          * Most affordable taxis (based on the taxi features in term of size, extra features and such)
          * Best rated taxis
      * Output:
          * A list of taxis sorted by preference (the maximum size is 10 taxis)
      
    * registerTaxi: It will register the taxi as available based on the filtering criteria selected.
      * Input:
        * Taxi
        * Current Taxi Location
        * Filter
          * Working Location Radio (in meters)
          * Shortest/Longest distance flag
          * Minimum customer rating
      * Output:
        * Expected to be void, throws an exception if anything goes wrong.
    
    * bookTaxi
      * Input
        * Taxi
        * Customer
        * Start Location
        * End Location
        * Price
      * Output
        * Returns the booking reference ID.

You can assume that there is already in place a Location class and a LocationService that given 2 Locations it returns the distance between both locations measured in meters.
Optional: implement Location and LocationService classes.
Optional: implement an API method to cancel a booking within certain time limit after it was
created (e.g. 10 minutes after the booking)

Try to implement the best data structures and algorithms possible to satisfy the features described (take into consideration that there might be over 1M taxis and 1M customers available at any moment). We are mainly interested in how you structure your code so to be easily extendable, complies with best OO practices and is easy for others to modify / understand.
Make sure you add comments for all the assumptions you have done while designing and implementing the code.

You can choose any OOP language. Please don’t use any framework or third party libraries as all the code must be written by you, with the exceptions of the usage of standard language libraries (STL for C++, Java Collections, Python Collections Module, etc) and the usage of a Framework for testing. Also note that no UI development is required.