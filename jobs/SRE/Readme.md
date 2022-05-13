# Mews SRE task

You are given a file `logs.txt` with JSON log items containing information about requests on production for some time interval. The item contains various data, but most importantly endpoint name and the request duration. Several deploys happened in the given interval with bugs that made some endpoints much slower (regressed).

Your task is to find which requests regressed and also the approximate time of it with 5-minute tolerance. Do it through either:
- a) writing a program in any language you would like (or even pseudocode) that reads this file and prints out the result.
- b) describe an approach how you would find the result.

Note that it is up to you to define what regression means.

**Input**: `logs.txt.` file.

**Output**: Name of endpoints that regressed and the time when it approximately happened.

Note that the quality of the code is not the most important aspect, since this is not a task for a backend engineer.
Since the task is pretty vague, it is also ok to make some reasonable assumptions about the form of regression. Just make sure to state them with your solution.
