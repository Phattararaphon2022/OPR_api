select * from SELF_MT_JOBTABLE where JOBTABLE_ID IN (21)
select * from SELF_TR_TIMELEAVE
select * from SELF_TR_APPROVE

update SELF_MT_JOBTABLE set STATUS_JOB = 'W' where JOBTABLE_ID IN (21)
update SELF_TR_TIMELEAVE set STATUS = 0
delete SELF_TR_APPROVE


SELECT * FROM ATT_TR_EMPLEAVEACC

DECLARE @worker_code varchar(20)= ''
DECLARE @year varchar(20)= ''
DECLARE @leave_code varchar(20)= ''
DECLARE @timeleave varchar(20)= ''
DECLARE @CompID varchar(20)= 'OPR'
SELECT @worker_code=WORKER_CODE, @year=YEAR(TIMELEAVE_FROMDATE) ,@leave_code=LEAVE_CODE,@timeleave=(TIMELEAVE_MIN/480) FROM SELF_TR_TIMELEAVE where COMPANY_CODE = 'OPR' 
AND TIMELEAVE_ID='1'
PRINT @worker_code
PRINT @year
PRINT @leave_code
PRINT @timeleave

UPDATE ATT_TR_EMPLEAVEACC SET EMPLEAVEACC_USED = EMPLEAVEACC_USED + @timeleave, EMPLEAVEACC_REMAIN = EMPLEAVEACC_REMAIN-@timeleave
WHERE COMPANY_CODE = @CompID AND YEAR_CODE=@year AND WORKER_CODE = @worker_code AND  LEAVE_CODE=@leave_code

select *  from SELF_TR_TIMELEAVE