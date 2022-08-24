import pandas as pd

if __name__ == "__main__":
    logs = pd.read_json("./logs.txt", lines=True)

    W = 100 # window size
    K = 3 # sensitivity of filter

    for endpoint in logs["Endpoint"].unique():
        # Select only the logs for this endpoint
        endpoint_logs = logs[logs["Endpoint"] == endpoint].copy()

        # Calculate rolling average and stdev of duration
        endpoint_logs["AverageDurationWindow"] = endpoint_logs["Duration"].rolling(W).mean().shift(1)
        endpoint_logs["StdevDurationWindow"] = endpoint_logs["Duration"].rolling(W).std().shift(1)

        # Find logs whose duration is bigger than (average_rolling_duration + K * stdev_rolling_duration)
        regressing_logs = endpoint_logs[endpoint_logs["Duration"] > endpoint_logs["AverageDurationWindow"] + endpoint_logs["StdevDurationWindow"] * K]
        if len(regressing_logs) > 1:
            print(f"{endpoint}: {len(regressing_logs)} found to be regressing")
            print(f"{endpoint}: first regressing request: {regressing_logs.iloc[0]['TimeStamp']}")
            print("")
