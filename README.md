## Problem context

You and your family had planned a dinner to the favourite restaurants, but as there are people like us who also want to visit for a dinner and at the end the number of people is more than the capacity of the restaurant and for this you have to wait for your turn and we people hate waiting. People also have another option to book the table through call and on the other side, the Restaurants staff writes your details on paper, But if the paper misplaced? there is no proof of your reservation. And Booking by call is also a waste of time of a restaurant as they have to attend the call at working time.

## Solution
The solution is Table Booking Apps which is convenient for both the owner and customers. For customers, it is easy to book a table and get to know it is available or not at your time. And for Restaurant owners or Staff, they don’t have to waste their time attending calls and save all the detail digitally which is easy to use. You can provide Pre-ordering of food which we discuss later in this article.

## What this project is for ?
This is just a pet project that represent a Proof of Concept where I make an attempt to combine DDD and Microservices patterns.

## WIP
App is currently not ready to run. There are services and other technical concerns that are not implemented yet. 
A lot of things are going to be implemented.
But if you would like to take a look at the code, you may start by looking and unit/integration tests to figure out what's going on 😉 

## Event storming

> Event storming is a workshop-based method to quickly find out what is happening in the domain of a software program (honestly copied from Wikipedia)

![EVent storming](https://github.com/shamil-sadigov/table-reservation-pet-project/blob/master/images/event%20storming.jpg)


PS: These are just simple events that are initial. Multiple additional (more intereseting) events will be added...

# Brief introduction to domain
__Precondition:__ Today is cloudy weather (just like I love), and it turned out that today is also Friday, so why not to visit a cafe ? These reasons are sufficient (at least for me). By the way, it turned out that cafe is type of restaurant, more than 20 years I didn't now that. If you are interested about other type of restaurants, they are [here](https://en.wikipedia.org/wiki/Types_of_restaurants)


So I open my app (table reservation app), choose a `Restaurnt` that I'm inteded to visit, select a `Table` that I'm going to reserve and send a `Reservation request`. So, I'm a `Visitor`, not a GoF pattern, but a person who is going to visit a Restaurant.

`Administrator` of `Restaurant` see that `Reservation request` that was sent by `Visitor`(me), and may take actions like `Approve` or `Reject`. Once `Reservation Request` is `Approved` then `Reservation` is made and `Table` is reserved for. Or `Administrator` may `Reject` `Reservation Request` by providing a reason. 


