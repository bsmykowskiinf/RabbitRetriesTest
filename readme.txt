1. use rabbitMQ 3.10.20
2. add "test" virtual host to rabbitMQ
3. import queues definitions to "test" virtual host from rabbit_testRetries_definitions.json file
4. set rabbitMQ connection string in CommonConfiguration.cs
5. set NSB license path in CommonConfiguration.cs
6. run TestPublisher project. It will load 100 messages to testRetries_Handler queue. 100 is default value but maybe have to be increased to reproduce bug, it depends on test environment performance.
7. run TestHandler project. It will try to handle messages, but at some point it will stuck in infinite immediate retry loop. 
  After few minutes check TestHandler/logs/. it will contain infinite immediate retry loop for some messages, like:

  2023-06-16 10:44:50.540 INFO  Immediate Retry is going to retry message 'cfbe808d-6dd6-4169-9533-b023008ea74c' because of an exception:
  2023-06-16 10:45:04.757 INFO  Immediate Retry is going to retry message 'cfbe808d-6dd6-4169-9533-b023008ea74c' because of an exception:
  2023-06-16 10:45:20.390 WARN  Delayed Retry will reschedule message 'cfbe808d-6dd6-4169-9533-b023008ea74c' after a delay of 00:00:10 because of an exception:
  2023-06-16 10:47:20.072 INFO  Immediate Retry is going to retry message 'cfbe808d-6dd6-4169-9533-b023008ea74c' because of an exception:
  2023-06-16 10:47:36.314 INFO  Immediate Retry is going to retry message 'cfbe808d-6dd6-4169-9533-b023008ea74c' because of an exception:
  2023-06-16 10:47:51.987 INFO  Immediate Retry is going to retry message 'cfbe808d-6dd6-4169-9533-b023008ea74c' because of an exception:
  2023-06-16 10:48:07.031 INFO  Immediate Retry is going to retry message 'cfbe808d-6dd6-4169-9533-b023008ea74c' because of an exception:
  2023-06-16 10:48:21.528 INFO  Immediate Retry is going to retry message 'cfbe808d-6dd6-4169-9533-b023008ea74c' because of an exception:


