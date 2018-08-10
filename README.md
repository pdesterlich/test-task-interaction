# test-task-interaction

## problem

A console app / windows service needs to start a number of separate tasks, each executing a continous loop; each task needs to receive events from an external source (via a signalr connection).

For performance reasons, handling a signalr connection for each task is not advisable; a better solution would be a single connection in the main thread, passing the received messages to all tasks.

## solution

this repository is a test app tha solve the problem.

- **send-message** is a console app to send a message to the signalr server (external to this project)
- **worker** is the test app

The worker starts 10 separate tasks, each writing a console message every 5 seconds and passing each task a BufferBlock queue; when the worker receive a message from the signalr server, the message is handled to each task via the queue.
