## Common
- Add Identity Provider service

## BuildingBlocks
- Add equality comparison based on id in Entity class (+ respective tests)
- Add code property to Error class so that Errors can be identified by some criteria
- Add unit tests for Result.CombineWith() method
- Add unit tests for ValueObject class to check comparison

## Restaurants.Domain
- Add unit test on restaurant, where we try to make reservation request while restaurant has no available table

## Visitor model
- Add more information about Visitor like Contact number, FirstName, LastName (... and maybe favorite rock band 😈)

# Steps TODO
- Update migrations in Reservation service
- Rename all domain events to end with 'DomainEvent'

# Optional
- - Add integration tests for repository
- When creataing reservation request we should be sure that visitor is trusted