# Coinbase Advanced Trade Bot

## Introduction

Coinbase Advanced Trade Bot is a .NET-based project that implements an automated trading strategy using the Coinbase Advanced Trade API. It is designed to be deployed as an Azure WebJob, allowing it to run at specified intervals for effective cryptocurrency trading.

## Requirements

- .NET 6.0 or higher
- Coinbase Advanced Trade API keys
- Newtonsoft.Json for JSON parsing
- Azure subscription for deploying WebJobs

## Installation

1. Clone the repository: `git clone https://github.com/your-repo/CoinbaseTradeStrategy.NET.git`
2. Navigate to the project directory and install dependencies.
3. Configure your Coinbase API keys in the application settings.

# Run locally

1. **Compile the Project**

   Run the following command to build the project:

   ```bash
   dotnet build
   ```

2. **Run the Application**

   Once the build is successful, you can run the application:

   ```bash
   dotnet run
   ```

   This will start the application and begin executing predefined strategies.

## Cloud Configuration

- Set up your Azure environment for WebJob deployment.
- Configure trade strategy parameters and intervals in the application settings.

## Deployment

1. Package the application for Azure.
2. Deploy the package to Azure WebJobs.
3. Set the scheduling for the WebJob according to your trading strategy needs.

## Usage

- The WebJob will automatically execute trades based on the defined strategy at the specified intervals.
- Monitor the WebJob logs for performance and transaction details.

## Features

- Automated trading based on predefined strategies.
- Real-time data fetching from Coinbase.
- Customizable trading intervals and parameters.
