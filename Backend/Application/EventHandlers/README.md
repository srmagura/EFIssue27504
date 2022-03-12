Service Bus delivers messages **at least once**, so all domain event handlers
need to be idempotent. I.e., if a message is received a second time, the 
event handler should do nothing.
