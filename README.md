# CurrencyConversion

## Select IIS from VS to Run project over local IIS Server

### exchangeRates.json file is placed with CurrencyConversion project to read all exchange rate values.

### to override exchange rate that we got from exchangeRates.json you need to set enviroment variables example set enviroment variable   USD_TO_INR:33 that will override the value found in exchangeRates.json file


### Middleware is configured so that if an error occur it will send the response in proper format and we can send custom response message in case of bad request or not found.

### There is an endpoint Convert/update-exchange-rate via which we can directly update values stored in exchangeRates.json so that we dont need to restart the application else we can also use alternate approach of getting those exchange value from any cloud service like azure key vault so that we dont need to restart the application for now we are reading those exchange values from exchangeRates.json file.
