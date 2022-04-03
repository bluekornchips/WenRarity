WITH CTE(fingerprint, 
    duplicatecount)
AS (SELECT Fingerprint,
           ROW_NUMBER() OVER(PARTITION BY [fingerprint]
           ORDER BY fingerprint) AS DuplicateCount
    FROM ChilledKongs)
DELETE FROM CTE
WHERE duplicatecount > 1;
SELECT *
FROM CTE;


select count(*) from ChilledKongs