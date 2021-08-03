## BuildingBlocks
- Add equality comparison based on id in Entity class
    - And respective tests
- Add code property to Error class so that Errors can be identified by some criteria
- Add unit tests for Result.CombineWith() method
- Add unit tests for ValueObject class to check comparison


## Reservation.Domain
- Add unit test on restaurant, where we try to make reservation request while restaurant has no available table


## Reservation.Application
- Duplicated commands should be returned as 409 status code (Conflick)
    - To achieve that we should make sure that all Error instances can be identified by some code.
        - And then we can create some factory on Presentation layer that will take an error and will return http status code based on Error's code
- Implement ResilientTransaction<TDbContext> class which injects DbContext and calls inside created execution strategy
and begins transaction

## Customer model
- Ensure customer model has phone number

# Steps TODO
- Add Administration model
- And then decide how to divide application by subdomain
- Rename all domain events to end with 'DomainEvent'
- Add Administration service
- Rename CanceledByCustomer to CanceledByVisitor
# Commong
-  Add Dependency extensions in each layer which would conigure all dependencie



# Optional
- - Add integration tests for repository
- When creataing reservation request we should be sure that visitor is trusted