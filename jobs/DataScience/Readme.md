# Mews data analyst task

## Your task

You have been given a task to look into data of one hotel in a form of following [YouTrack](https://www.jetbrains.com/youtrack/) issue:

-------------

**Summary**: Analyse reservations of this hotel

**Description**:

1) What are the popular choices of booking rates (table `rate`, columns `ShortRateName` or `RateName`) for different segments of customers (table `reservation`, columns `AgeGroup`, `Gender`, `NationalityCode`)?

2) What are the typical guests who do online check-in? Is it somehow different when you compare reservations created across different weekdays (table `reservation`, `IsOnlineCheckin` column)?

3) Look at the average night revenue per single occupied capacity. What guest segment is the most profitable per occupied space unit? And what guest segment is the least profitable?

------------

There are three scenarios that you should provide solutions for:

In the first scenario, this task is assigned to you by a tech lead, he already commented on the task that he needed a one-time ad-hoc analysis. For the purpose of this interview task, use SQL.

In the second scenario, this task is assigned to you by a product manager who wants you to build a report so that she can keep track of the situation and it's easy for her to check the answers. Also she mentioned she'll have few more questions later. For the purpose of this interview task, we prefer you use Power BI. You may also use Excel, jupyter notebook, or similar tool.

In the third scenario, we want to motivate our hotels to promote online checkin and we want to give them some hard data. Look at the data and your analysis again and estimate what would be the impact on total room revenue if the overall usage of online checkin doubled.

Include the comments to present your solution and send it to vojta@mews.com. 

Thanks and good luck.

## Assets

In the `./Task` folder, you find two CSV files `rates` and `reservations` that you can load into SQL database of your choice, or you can use `import_mssql.sql` file that contains the same data in the form of queries to create the db tables and insert all the data.

## Specification

For 3rd task, you will use `NightCost_Sum` (i.e. room revenue) and `OccupiedSpace_Sum` columns. Since the data already contains aggregated values, here is how the `OccupiedSpace_Sum` is calculated:


```sql
SELECT 
 ...
 ISNULL(SUM(RoomCategory.[Capacity] + RoomCategory.[ExtraCapacity]), 0) as OccupiedSpace_Sum,
 ...
FROM Reservation
LEFT JOIN Room ON (Room.Id = Reservation.AssignedRoomId)
LEFT JOIN RoomCategory ON (RoomCategory.ID = Room.CategoryId) 
```

where `Capacity` is pretty much how many people you can fit into the space, or more vaguely how many beds are there.
