USE [OPR]
GO
/****** Object:  StoredProcedure [dbo].[SELF_MT_APPROVEDOC]    Script Date: 13/06/2023 16:30:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [dbo].[SELF_MT_APPROVEDOC]
(	
	@CompID varchar (30),
	@JobID varchar (30),
	@JobType varchar (30),
	@Username varchar (20),
	--@Language varchar (2),
	@ApproveStatus varchar (10)
	--@ApproveDescription varchar (100)
)
  
AS  
BEGIN 

--DECLARE @CompID varchar (30) = 'OPR'
--DECLARE @JobID varchar (30) = '21'
--DECLARE @JobType varchar (30) = 'LEA'
--DECLARE @Username varchar (20) = 'Approve01'
----DECLARE @Language varchar (2) = 'T'
--DECLARE @ApproveStatus varchar (10) = 'A'
----DECLARE @ApproveDescription varchar (100) = ''

DECLARE @Message varchar (200) = ''
PRINT 'test'
-- Step 1 Get Approve Detail
DECLARE @userLevel int
SELECT @userLevel = ISNULL(ACCOUNT_LEVEL, 1)
FROM SELF_MT_ACCOUNT
WHERE COMPANY_CODE=@CompID AND ACCOUNT_TYPE='APR' AND ACCOUNT_USER=@Username

PRINT 'Username : ' + @Username
PRINT 'Level : ' + CAST(@userLevel AS varchar(30))

-- Step 2 Check Approved is username
DECLARE @count_approved int = 0
SELECT @count_approved=ISNULL(COUNT(APPROVE_ID), 0)
FROM SELF_TR_APPROVE
WHERE COMPANY_CODE=@CompID
AND APPROVE_USER = @Username
AND JOB_TYPE = @JobType
AND JOBTABLE_ID = @JobID

PRINT 'Approved : ' + CAST(@count_approved AS varchar(30))

IF @count_approved > 0 BEGIN
	SET @Message = '-- You have already Approve. --'
	PRINT @Message
	return 101
END

-- Step 3 Check Approved is position
DECLARE @position int = 0
SELECT
@position=ISNULL(COUNT(SELF_TR_LINEAPPROVE.POSITION_LEVEL), 0)
FROM SELF_MT_JOBTABLE
JOIN SELF_TR_LINEAPPROVE ON SELF_TR_LINEAPPROVE.COMPANY_CODE = SELF_MT_JOBTABLE.COMPANY_CODE 
AND SELF_TR_LINEAPPROVE.WORKFLOW_CODE = SELF_MT_JOBTABLE.WORKFLOW_CODE
WHERE SELF_MT_JOBTABLE.COMPANY_CODE = @CompID
AND JOBTABLE_ID = @JobID
AND JOB_TYPE = @JobType
AND SELF_TR_LINEAPPROVE.POSITION_LEVEL IN (
SELECT DISTINCT EMP_MT_POSITION.POSITION_LEVEL FROM SELF_TR_ACCOUNTPOS
JOIN  EMP_MT_POSITION ON EMP_MT_POSITION.COMPANY_CODE = SELF_TR_ACCOUNTPOS.COMPANY_CODE
AND EMP_MT_POSITION.POSITION_CODE = SELF_TR_ACCOUNTPOS.POSITION_CODE
WHERE SELF_TR_ACCOUNTPOS.ACCOUNT_USER = @Username)

IF @position = 0 BEGIN
	SET @Message = '-- This user'+ CAST(@Username AS varchar(30)) +' has been approved --'
	PRINT @Message
	return 101
END

-- Step 4 Get Workflow 
DECLARE @totalApprove int
SELECT @totalApprove = TOTALAPPROVE
FROM SELF_MT_JOBTABLE
INNER JOIN SELF_MT_WORKFLOW ON SELF_MT_JOBTABLE.COMPANY_CODE=SELF_MT_WORKFLOW.COMPANY_CODE AND SELF_MT_JOBTABLE.WORKFLOW_CODE=SELF_MT_WORKFLOW.WORKFLOW_CODE AND SELF_MT_JOBTABLE.JOB_TYPE=SELF_MT_WORKFLOW.WORKFLOW_TYPE
WHERE SELF_MT_JOBTABLE.COMPANY_CODE=@CompID
AND SELF_MT_JOBTABLE.JOB_TYPE = @JobType
AND SELF_MT_JOBTABLE.JOBTABLE_ID = @JobID

-- Step 5 Count TotalApprove
DECLARE @count_totalapproved int = 0
SELECT @count_totalapproved=ISNULL(COUNT(APPROVE_ID), 0)
FROM SELF_TR_APPROVE
WHERE COMPANY_CODE=@CompID
AND JOB_TYPE = @JobType
AND JOBTABLE_ID = @JobID

PRINT 'Total approve : ' + CAST(@totalApprove AS varchar(30))
PRINT 'Sum approved : ' + CAST(@count_totalapproved AS varchar(30))

IF @count_totalapproved = @totalApprove BEGIN
	SET @Message = '-- The document has been approved. --'
	PRINT @Message
	return 103
END

-- Step 6 Approve doc
DECLARE @record bit = 0
BEGIN TRY 	
	DECLARE @approve_id int = 0;
	SELECT @approve_id=ISNULL(MAX(APPROVE_ID),0) FROM SELF_TR_APPROVE
	SET @approve_id = @approve_id + 1
	PRINT 'APPROVE_ID : ' + CAST(@approve_id AS varchar(30))

	INSERT INTO SELF_TR_APPROVE (APPROVE_ID, COMPANY_CODE, JOB_TYPE, APPROVE_USER, APPROVE_DATE, APPROVE_LEVEL,JOBTABLE_ID)
	VALUES (@approve_id,@CompID, @JobType,@Username,getdate(),@userLevel, @JobID)
	
	SET @record = 1
	SET @count_totalapproved = @count_totalapproved + 1

END TRY  
BEGIN CATCH		
    SET @record = 0
	SET @Message = '-- [BC001] There was a problem approving the data. --'
	PRINT @Message		
END CATCH

PRINT 'Record : ' + CAST(@record AS varchar(30))


DECLARE @StatusJob varchar (20) = 'W'
DECLARE @record_hrfocus bit = 1
IF @count_totalapproved = @totalApprove OR @ApproveStatus='C'  BEGIN
	DECLARE @status_timedoc int = 0
	IF @ApproveStatus = 'C' BEGIN
		SET @status_timedoc = 4
		SET @StatusJob = 'C'
	END
	IF @ApproveStatus = 'A' BEGIN
		SET @status_timedoc = 3
		SET @StatusJob = 'F'
	END
	-- Record to HRFocus
	BEGIN TRY
			
		IF @JobType = 'LEA' BEGIN
			DECLARE @timeleave_id varchar (30) = '0'
			SELECT @timeleave_id=JOB_ID FROM SELF_MT_JOBTABLE WHERE COMPANY_CODE=@CompID AND JOBTABLE_ID=@JobID AND JOB_TYPE=@JobType
			UPDATE SELF_TR_TIMELEAVE SET STATUS = @status_timedoc
			WHERE COMPANY_CODE=@CompID AND TIMELEAVE_ID=@timeleave_id
			IF @ApproveStatus = 'C' BEGIN
				DECLARE @worker_code varchar(20)= ''
				DECLARE @year varchar(20)= ''
				DECLARE @leave_code varchar(20)= ''
				DECLARE @timeleave varchar(20)= ''
				SELECT @worker_code=WORKER_CODE, @year=YEAR(TIMELEAVE_FROMDATE) ,@leave_code=LEAVE_CODE,@timeleave=(TIMELEAVE_MIN/480) FROM SELF_TR_TIMELEAVE where COMPANY_CODE = @CompID
				AND TIMELEAVE_ID=@timeleave_id

				UPDATE ATT_TR_EMPLEAVEACC SET EMPLEAVEACC_USED = EMPLEAVEACC_USED-@timeleave, EMPLEAVEACC_REMAIN = EMPLEAVEACC_REMAIN+@timeleave
				WHERE COMPANY_CODE = @CompID AND YEAR_CODE=@year AND WORKER_CODE = @worker_code AND  LEAVE_CODE=@leave_code
			END
		END

		IF @JobType = 'OT' BEGIN
			DECLARE @timeot_id varchar (30) = '0'
			SELECT @timeot_id=JOB_ID FROM SELF_MT_JOBTABLE WHERE COMPANY_CODE=@CompID AND JOBTABLE_ID=@JobID AND JOB_TYPE=@JobType
			UPDATE SELF_TR_TIMEOT SET STATUS = @status_timedoc
			WHERE COMPANY_CODE=@CompID AND TIMEOT_ID=@timeot_id
		END

		IF @JobType = 'SHT' BEGIN
			DECLARE @timeshift_id varchar (30) = '0'
			SELECT @timeshift_id=JOB_ID FROM SELF_MT_JOBTABLE WHERE COMPANY_CODE=@CompID AND JOBTABLE_ID=@JobID AND JOB_TYPE=@JobType
			UPDATE SELF_TR_TIMESHIFT SET STATUS = @status_timedoc
			WHERE COMPANY_CODE=@CompID AND TIMESHIFT_ID=@timeshift_id

		END

		IF @JobType = 'ONS' BEGIN
			DECLARE @timesh_id varchar (30) = '0'
			SELECT @timesh_id=JOB_ID FROM SELF_MT_JOBTABLE WHERE COMPANY_CODE=@CompID AND JOBTABLE_ID=@JobID AND JOB_TYPE=@JobType
			UPDATE SELF_TR_TIMEONSITE SET STATUS = @status_timedoc
			WHERE COMPANY_CODE=@CompID AND TIMEONSITE_ID=@timesh_id
		END

		IF @JobType = 'DAT' BEGIN
			DECLARE @timedaytype_id varchar (30) = '0'
			SELECT @timedaytype_id=JOB_ID FROM SELF_MT_JOBTABLE WHERE COMPANY_CODE=@CompID AND JOBTABLE_ID=@JobID AND JOB_TYPE=@JobType
			UPDATE SELF_TR_TIMEDAYTYPE SET STATUS = @status_timedoc
			WHERE COMPANY_CODE=@CompID AND TIMEDAYTYPE_ID=@timedaytype_id
			
		END
		IF @JobType = 'CI' BEGIN
			DECLARE @timecheckin_id varchar (30) = '0'
			SELECT @timecheckin_id=JOB_ID FROM SELF_MT_JOBTABLE WHERE COMPANY_CODE=@CompID AND JOBTABLE_ID=@JobID AND JOB_TYPE=@JobType
			UPDATE SELF_TR_TIMECHECKIN SET STATUS = @status_timedoc
			WHERE COMPANY_CODE=@CompID AND TIMECHECKIN_ID=@timecheckin_id
		END

		IF @JobType = 'REQ' BEGIN
			DECLARE @timereqdoc_id varchar (30) = '0'
			SELECT @timereqdoc_id=JOB_ID FROM SELF_MT_JOBTABLE WHERE COMPANY_CODE=@CompID AND JOBTABLE_ID=@JobID AND JOB_TYPE=@JobType
			UPDATE SELF_MT_REQDOC SET STATUS = @status_timedoc
			WHERE COMPANY_CODE=@CompID AND REQDOC_ID=@timereqdoc_id
		END
		SET @Message = '-- Send document to HRFocus. --'
		PRINT @Message		

	END TRY  
	BEGIN CATCH		
		SET @Message = '-- [BC002] There was a problem approving the data. --'
		PRINT @Message		
		SET @record_hrfocus = 0
	END CATCH	

END


IF @record=1 AND @record_hrfocus=1 BEGIN
		
	UPDATE SELF_MT_JOBTABLE SET STATUS_JOB = @StatusJob ,JOB_FINISHDATE = getdate()
	WHERE COMPANY_CODE=@CompID AND JOB_TYPE=@JobType AND JOBTABLE_ID=@JobID
	 
	PRINT 'Approve Complete!'

	RETURN 111
END
ELSE BEGIN

	DELETE FROM SELF_TR_APPROVE
	WHERE COMPANY_CODE=@CompID AND JOB_TYPE=@JobType AND JOBTABLE_ID=@JobID AND APPROVE_USER=@Username

	--DELETE FROM tbMTWebJobTran
	--WHERE ComID=@CompID AND JobType=@JobType AND JobID=@JobID AND UserName=@Username

	SET @StatusJob = 'W'

	UPDATE SELF_MT_JOBTABLE SET STATUS_JOB = @StatusJob
	WHERE COMPANY_CODE=@CompID AND JOB_TYPE=@JobType AND JOBTABLE_ID=@JobID

	RETURN 999
END

END  

