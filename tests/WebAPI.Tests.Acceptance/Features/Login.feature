@Login
Feature: Login
    User can log in

Scenario: User can log in with valid credentials
    When a logged out user logs in with valid credentials
      | Email                   | Password        |
      | administrator@localhost | Administrator1! |
    Then they log in successfully
    
Scenario: User cannot log in with invalid credentials
    When a logged out user logs in with invalid credentials
      | Email            | Password            |
      | hacker@localhost | BigHackerKitLau! |
    Then an error is display 
