
/**********QUESTION 1**********/
--What are the popular choices of booking rates (table `rate`, columns `ShortRateName` or `RateName`) for different segments of customers (table `reservation`, columns `AgeGroup`, `Gender`, `NationalityCode`)?

--Popular booking rates for each AgeGroup &  Gender & Nationality combinations (626 different segments)
WITH CTE AS
		(SELECT
			ISNULL(NationalityCode,'Unkown') as Nationality,
			CASE
				WHEN Gender='0' THEN 'Unknown'
				WHEN Gender='1' THEN 'Male'
				ELSE 'Female'
			END AS Gender,
			CASE
				WHEN AgeGroup='0' THEN 'Unknown'
				WHEN AgeGroup='25' THEN '25'
				WHEN AgeGroup='35' THEN '35'
				WHEN AgeGroup='45' THEN '45'
				WHEN AgeGroup='55' THEN '55'
				WHEN AgeGroup='65' THEN '65'
				ELSE '100'
			END AS AgeGroup,
			RateName,
			COUNT(*) AS "Number of Reservations",
			CONCAT(CAST(CAST(COUNT(*) AS DECIMAL(5,2))*100/ SUM(COUNT(*)) OVER(PARTITION BY NationalityCode, Gender, AgeGroup) AS DECIMAL(5,1)),' %') AS "Percent of Total"
		FROM
			reservation
		LEFT JOIN
			rate
			ON reservation.RateId=rate.RateId
		GROUP BY
			AgeGroup,
			Gender,
			NationalityCode,
			RateName)
SELECT
	*,
	ROW_NUMBER() OVER(PARTITION BY AgeGroup,Gender, Nationality ORDER BY "Number of Reservations" DESC) AS Popularity
FROM
	CTE
ORDER BY
	Nationality,
	Gender,
	AgeGroup, 
	Popularity
/*
I would suggest looking at the the results seperately to see a wider picture.
	--Insights for Age: For every age group the most popular booking rate is Fully Flexible, except age group 100.  Usage of Fully Flexible booking rate is getting stronger with the age.
	--Insights for Gender: Fully Flexible is the most popular booking rate for gender groups Male and Female, while it's Early 60days for gender group "unknown".
*/

--Popular booking rates for each AgeGroup &  Gender & Nationality(Focused on majority of nationalities: US,GB,DE,SK,CZ)
--Nationality increases the number of segment, I would suggest looking at the results for nationalities where we have more data
WITH CTE AS
		(SELECT
			ISNULL(NationalityCode,'Unkown') as Nationality,
			CASE
				WHEN Gender='0' THEN 'Unknown'
				WHEN Gender='1' THEN 'Male'
				ELSE 'Female'
			END AS Gender,
			CASE
				WHEN AgeGroup='0' THEN 'Unknown'
				WHEN AgeGroup='25' THEN '25'
				WHEN AgeGroup='35' THEN '35'
				WHEN AgeGroup='45' THEN '45'
				WHEN AgeGroup='55' THEN '55'
				WHEN AgeGroup='65' THEN '65'
				ELSE '100'
			END AS AgeGroup,
			RateName,
			COUNT(*) AS "Number of Reservations",
			CONCAT(CAST(CAST(COUNT(*) AS DECIMAL(5,2))*100/ SUM(COUNT(*)) OVER(PARTITION BY NationalityCode, Gender, AgeGroup) AS DECIMAL(5,1)),' %') AS "Percent of Total"
		FROM
			reservation
		LEFT JOIN
			rate
			ON reservation.RateId=rate.RateId
		GROUP BY
			AgeGroup,
			Gender,
			NationalityCode,
			RateName)
SELECT
	*,
	ROW_NUMBER() OVER(PARTITION BY AgeGroup,Gender, Nationality ORDER BY "Number of Reservations" DESC) AS Popularity
FROM
	CTE
WHERE
	Nationality IN ('US','GB','DE','SK','CZ')
ORDER BY
	Nationality,
	Gender,
	AgeGroup, 
	Popularity
--Insights: The most preferred booking rate is Fully Flexible for all customer from these 5 countries, however, usage of it is very strong CZ compared to other countries with 75% of the customer preference.



/**********QUESTION 2**********/
--What are the typical guests who do online check-in? Is it somehow different when you compare reservations created across different weekdays (table `reservation`, `IsOnlineCheckin` column)?

/*
Number of data for online-check-in is low and it is quite spread across the nationalities.I would suggest looking at the data set by age, gender and nationality seperately.
This question can be answered by building many pivot tables using different kind of attributes.
Note: Data quality is better for reservations with online check-in since email,nationality,gender etc. info captured(no missing data).
*/

--Age Group
--Unknowns are excluded
SELECT
	AgeGroup,
	COUNT(*) AS "Number of Reservations",
	CONCAT(CAST(CAST(COUNT(*) AS DECIMAL(5,2))*100/ SUM(COUNT(*)) OVER() AS DECIMAL(5,1)),' %') AS "Percent of Total"
FROM
	reservation
WHERE 
	IsOnlineCheckin = 1
	AND
	AgeGroup<>0
GROUP BY
	AgeGroup
ORDER BY
	"Number of Reservations" DESC
--Insights:Online check-in customers are mostly people with 35 / 45 / 25 age group


--Gender
--There isn't any unknown gender in online-check in customers already.
SELECT
	CASE
		WHEN Gender='0' THEN 'Unknown'
		WHEN Gender='1' THEN 'Male'
		ELSE 'Female'
	END AS Gender,
	COUNT(*) AS "Number of Reservations",
	CONCAT(CAST(CAST(COUNT(*) AS DECIMAL(5,2))*100/ SUM(COUNT(*)) OVER() AS DECIMAL(5,1)),' %') AS "Percent of Total"
FROM
	reservation
WHERE 
	IsOnlineCheckin = 1
GROUP BY
	Gender
ORDER BY
	"Number of Reservations" DESC
--Insights:Majority of online check-in customers are males.(Like it's in offline check-in customers)


--Nationality
--Unknowns are excluded
SELECT
	NationalityCode,
	COUNT(*) AS "Number of Reservations",
	CONCAT(CAST(CAST(COUNT(*) AS DECIMAL(5,2))*100/ SUM(COUNT(*)) OVER() AS DECIMAL(5,1)),' %') AS "Percent of Total"
FROM
	reservation
WHERE 
	IsOnlineCheckin = 1
	AND
	NationalityCode IS NOT NULL
GROUP BY
	NationalityCode
ORDER BY
	"Number of Reservations" DESC
--Insights: Main customers are from US,GB,FR,...



--Day of week
SELECT
	DATENAME(WEEKDAY, CreatedUtc) "Day of the Week",
	COUNT(*) AS "Number of Reservations",
	CONCAT(CAST(CAST(COUNT(*) AS DECIMAL(5,2))*100/ SUM(COUNT(*)) OVER() AS DECIMAL(5,1)),' %') AS "Percent of Total"
FROM
	reservation
GROUP BY
	DATENAME(WEEKDAY, CreatedUtc)
ORDER BY
	"Number of Reservations" DESC
--Insights: It's not equally distributed, weekdays are preferred to book an availability against weekends for revervations



/**********QUESTION 3**********/
--Look at the average night revenue per single occupied capacity. What guest segment is the most profitable per occupied space unit? And what guest segment is the least profitable?

--Segments by Nationality & Gender & Age Group (Cancelled reservations and reservations which the customers did not show up are excluded.)
--The most profitable segment according to the defined metric (Please be aware of that the number of reservation is only 1)
SELECT
	TOP 1
	NationalityCode,
	CASE
		WHEN AgeGroup='0' THEN 'Unknown'
		WHEN AgeGroup='25' THEN '25'
		WHEN AgeGroup='35' THEN '35'
		WHEN AgeGroup='45' THEN '45'
		WHEN AgeGroup='55' THEN '55'
		WHEN AgeGroup='65' THEN '65'
		ELSE '100'
	END AS AgeGroup,
	CASE
		WHEN Gender='0' THEN 'Unknown'
		WHEN Gender='1' THEN 'Male'
		ELSE 'Female'
	END AS Gender,
	Count(*) as "Number of Reservations",
	CAST(SUM(NightCost_Sum)/SUM(OccupiedSpace_Sum) AS DECIMAL(5,1)) as "Avg Revenue per Occ Cap"
FROM
	reservation
WHERE
	OccupiedSpace_Sum <> 0
GROUP BY
	NationalityCode,
	AgeGroup,
	Gender
ORDER BY
	"Avg Revenue per Occ Cap" DESC


--Segments by Nationality & Gender & Age Group (Cancelled reservations and reservations which the customers did not show up are excluded.)
--The least profitable segment according to the defined metric 
SELECT
	TOP 1
	NationalityCode,
	CASE
		WHEN AgeGroup='0' THEN 'Unknown'
		WHEN AgeGroup='25' THEN '25'
		WHEN AgeGroup='35' THEN '35'
		WHEN AgeGroup='45' THEN '45'
		WHEN AgeGroup='55' THEN '55'
		WHEN AgeGroup='65' THEN '65'
		ELSE '100'
	END AS AgeGroup,
	CASE
		WHEN Gender='0' THEN 'Unknown'
		WHEN Gender='1' THEN 'Male'
		ELSE 'Female'
	END AS Gender,
	Count(*) as "Number of Reservations",
	CAST(SUM(NightCost_Sum)/SUM(OccupiedSpace_Sum) AS DECIMAL(5,1)) as "Avg Revenue per Occ Cap"
FROM
	reservation
WHERE
	OccupiedSpace_Sum <> 0
GROUP BY
	NationalityCode,
	AgeGroup,
	Gender
ORDER BY
	"Avg Revenue per Occ Cap" ASC

--Some other insights according to the defined metric
--Insights: The most profitable nationality is DE, the least is SK in 5 major nationalities (US,GB,DE,SK,CZ)
--Insights: The most profitable Age group is 55, the least is Age group 65.
--Insights: The most profitable Gender group is Females, followed by Males, and Unknowns.