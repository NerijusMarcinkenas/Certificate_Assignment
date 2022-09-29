# Certificates assigment

## Background

__Warranty Expert__ is insurance agent company that allows shops to provide insurance and warranty services for their end customers.

Once service is purchased - customers receive insurance certificate that holds information about the insured item, customer and how long the certificate is valid.

## Task
Fork this repo and finish implementing insurance certificate creation in the backend that satisfies business and technical requirements.

Use Visual Studio or Visual Studio Code to launch this project. It will launch both .NET backend and Angular apps.

### Business requirements
* Customer must be at least 18 years old when buying insurance certificate.
* Certificate is considered valid from the moment it is created and lasts 1 year. The last day is valid for the whole day. For example if a customer buys certificate on __2022-09-01 14:30__, then certificate is valid from __2022-09-01 14:30__ until 
__2023-09-01 00:00__.
* Certificate number consists of 5 digits. Each number must be unique and incremented by 1. For example first certificates will have these numbers: 00001, 00002, 00003.
* Certificate sum depends on insured item sum. For example if insured item sum is 75 then certificate sum is 15. If insured item sum does not fit any range - then certificate must not be allowed to be created.

| Insured item price range | Certificate sum |
| --- | ----------- |
| 20.00 - 50.00 | 8 |
| 50.01 - 100.00 | 15 |
| 100.01 - 200.00 | 25 |

### Technical requirements
* Error message must be provided in case of validation error
* Certificate must be saved and provided in the list after successful validation

## Evaluation criteria
* Business and technical requirement fulfilment
* Code readability (variables, method names, class names)
* Code formatting
* Code structure
* Business requirment unit testability
* Bonus for unit tests
