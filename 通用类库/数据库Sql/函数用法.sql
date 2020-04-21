


--逗号分割字段值
CREATE  VIEW V_Jusoft_SplitDingName  
AS   
SELECT b.IdRestaurant ,STUFF(  
       (  
           SELECT ',' + Name  
           FROM  
           (  
               SELECT a.IdRestaurant,  
                      a.IdPerson,  
                      b.Name  
               FROM REF_Restaurant_Person a  
                   LEFT JOIN dbo.OR_Person b  
                       ON a.IdPerson = b.Id  
           ) AS a  WHERE a.IdRestaurant=b.IdRestaurant   
           FOR XML PATH('')  
       ),  
       1,  
       1,  
       '') AS DingName,  
    STUFF(  
       (  
           SELECT ',' + a.PsnNum  
           FROM  
           (  
               SELECT a.IdRestaurant,  
                      a.IdPerson,  
                      b.Name,  
       b.PsnNum   
               FROM REF_Restaurant_Person a  
                   LEFT JOIN dbo.OR_Person b  
                       ON a.IdPerson = b.Id  
           ) AS a  WHERE a.IdRestaurant=b.IdRestaurant   
           FOR XML PATH('')  
       ),  
       1,  
       1,  
       '') AS DingIdPerson  
     FROM  (SELECT IdRestaurant FROM  REF_Restaurant_Person  GROUP BY IdRestaurant) AS b