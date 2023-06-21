
/*==========================================================================================================================*/
/*                                                             TRIGGER                                                      */
/*==========================================================================================================================*/

CREATE TRIGGER tr_InsertBillDetails ON tbl_BillDetails
AFTER INSERT
AS
BEGIN
	declare @idBill int
	set @idBill = (select IDBILL from inserted i)
	update TBL_BILL
	set TOTALMONEY = TOTALMONEY + (select p.PRICE from inserted i, TBL_TICKET t, TBL_PRICE p where i.IDTICKET = t.IDTICKET and t.IDPRICE = p.IDPRICE)
	where IDBILL = @idBill
END
GO
CREATE TRIGGER tr_DeleteBillDetails ON tbl_BillDetails
AFTER DELETE
AS
BEGIN
	declare @idBill int
	set @idBill = (select IDBILL from deleted d)
	update TBL_BILL
	set TOTALMONEY = TOTALMONEY - (select p.PRICE from deleted d, TBL_TICKET t, TBL_PRICE p where d.IDTICKET = t.IDTICKET and t.IDPRICE = p.IDPRICE)
	where IDBILL = @idBill
END
GO

CREATE TRIGGER tr_InsertEmployee ON tbl_Employee
AFTER INSERT
AS
BEGIN
	declare @idPermissionGroup int
	select @idPermissionGroup = case
									when ty.NAMETYPE like N'%Quản Lý%' then (select IDPERMISSIONGROUP from TBL_PERMISSIONGROUP p where p.NAMEGROUP = N'Admin')
									when ty.NAMETYPE like N'%Kế Toán%' then (select IDPERMISSIONGROUP from TBL_PERMISSIONGROUP p where p.NAMEGROUP = N'Kế Toán')
									when ty.NAMETYPE like N'%Bán vé%' then (select IDPERMISSIONGROUP from TBL_PERMISSIONGROUP p where p.NAMEGROUP = N'Bán Vé')
									else 0
								end from inserted i, TBL_TYPEOFEMPLOYEE ty where i.IDTYPE = ty.IDTYPE
	declare @username varchar(max), @password varchar(max) = 'employee123', @idEmployee int
	select @idEmployee = e.IDEMPLOYEE, @username = e.EMAILEMPLOYEE from inserted e
	
	if @idPermissionGroup = 0
	begin
		update TBL_EMPLOYEE set IDPERMISSIONGROUP = (select IDPERMISSIONGROUP from TBL_PERMISSIONGROUP p where p.NAMEGROUP = N'Không') where IDEMPLOYEE = @idEmployee
		return
	end
	
	insert into tbl_Account(USERNAME, PASSWORD, IDEMPLOYEE) values (@username, @password, @idEmployee)
	declare @idaccount int
	select @idaccount = IDACCOUNT from TBL_ACCOUNT where USERNAME = @username and PASSWORD = @password
	update TBL_EMPLOYEE set IDACCOUNT = @idaccount, IDPERMISSIONGROUP = @idPermissionGroup where IDEMPLOYEE = @idEmployee
END	
GO
CREATE TRIGGER tr_UpdateAmountSeatEmptyTrip ON tbl_Booked
AFTER INSERT
AS
BEGIN
	declare @idTrip int
	set @idTrip = (select i.IDTRIP from inserted i)
	update TBL_TRIP
	set AMOUNTEMPTYSEAT = AMOUNTEMPTYSEAT - 1
	where IDTRIP = @idTrip
END
CREATE TRIGGER tr_UpdateAmountSeatEmptyTrip_Delete ON tbl_Booked
AFTER DELETE
AS
BEGIN
	declare @idTrip int
	set @idTrip = (select d.IDTRIP from deleted d)
	update TBL_TRIP
	set AMOUNTEMPTYSEAT = AMOUNTEMPTYSEAT + 1
	where IDTRIP = @idTrip
END


/*==========================================================================================================================*/
/*                                                              VIEW                                                        */
/*==========================================================================================================================*/
CREATE VIEW view_trip
AS
	select t.IDTRIP, cast(ti.STARTTIME as nvarchar(max)) + ' - ' + cast(ti.FINISHTIME as nvarchar(max)) as 'Time', b.DEPARTURESTATION + ' - ' + b.DESTINATIONSTATION as 'BusLine', e.NAMEEMPLOYEE, c.LICENSEPLATE, d.NAMEDRIVER, t.DEPARTUREDAY, t.AMOUNTEMPTYSEAT
	from TBL_TRIP t, TBL_TIMEBUSLINE ti, TBL_BUSLINE b, TBL_EMPLOYEE e, TBL_COACH c, TBL_DRIVER d
	where ti.IDTIME = t.IDTIME and b.IDBUSLINE = t.IDBUSLINE and e.IDEMPLOYEE = t.IDEMPLOYEE and c.IDCOACH = t.IDCOACH and d.IDDRIVER = t.IDDRIVER
GO
CREATE VIEW view_Bill
AS
	select bd.IDBILL, tic.IDTICKET, t.IDBUSLINE, b.DEPARTURESTATION, b.DESTINATIONSTATION, s.SEATPOSITION, c.LICENSEPLATE, ti.STARTTIME, ti.FINISHTIME, convert(varchar(max), t.DEPARTUREDAY, 103) AS 'DEPARTUREDAY', p.PRICE, convert(varchar(max), tic.TICKETPURCHASEDATE, 103) AS 'TICKETPURCHASEDATE', e.NAMEEMPLOYEE 
    from TBL_BILL, TBL_BILLDETAILS bd, TBL_TICKET tic, TBL_TRIP t, TBL_BUSLINE b, TBL_SEAT s, TBL_TIMEBUSLINE ti, TBL_PRICE p, TBL_EMPLOYEE e, TBL_COACH c
    where TBL_BILL.IDBILL = bd.IDBILL and bd.IDTICKET = tic.IDTICKET and tic.IDTRIP = t.IDTRIP and t.IDCOACH = c.IDCOACH and t.IDBUSLINE = b.IDBUSLINE and t.IDTIME = ti.IDTIME and TBL_BILL.IDEMPLOYEE = e.IDEMPLOYEE and tic.IDSEAT = s.IDSEAT
GO
CREATE VIEW view_BaoCao
AS
	select d.IDDRIVER, convert(datetime,t.DEPARTUREDAY, 103) as 'DEPARTUREDAY', d.NAMEDRIVER, d.PHONEDRIVER, b.DEPARTURESTATION, b.DESTINATIONSTATION, ve.SoVe, ve.TOTALMONEY, c.LICENSEPLATE
	from TBL_DRIVER d, TBL_TRIP t, TBL_COACH c, TBL_BUSLINE b,
		(select tic.IDTRIP, count(*) as 'SoVe', b.TOTALMONEY from TBL_TICKET tic, TBL_TRIP t, TBL_BILL b, TBL_BILLDETAILS bd where tic.IDTRIP = t.IDTRIP and tic.IDTICKET = bd.IDTICKET and bd.IDBILL = b.IDBILL group by b.TOTALMONEY, tic.IDTRIP) as ve
	where t.IDDRIVER = d.IDDRIVER and t.IDCOACH = c.IDCOACH and t.IDBUSLINE = b.IDBUSLINE and t.IDTRIP = ve.IDTRIP
GO

/*==========================================================================================================================*/
/*                                                       STORE PROCEDURE                                                    */
/*==========================================================================================================================*/
CREATE PROC sp_TimeDistance @Start time, @End time
AS
	SELECT CONVERT(TIME,DATEADD(MS,DATEDIFF(SS, @Start, @End)*1000,0),114)
GO

CREATE PROC sp_GetIDAccount @username nvarchar(max), @password nvarchar(max)
AS
	declare @id int = 0
	if (select count(*) from tbl_Account where Username = @username and Password = @password) > 0
		select @id = IDACCOUNT from tbl_Account where Username = @username and Password = @password
	return @id
GO

CREATE PROC sp_InsertTrip @idTime int, @departureday varchar(max), @idBusLine int, @idEmployee int, @idCoach int, @idDriver int, @strResult nvarchar(max) output
AS
	if(DATEDIFF(D, GETDATE(), convert(date,@departureday,103)) < 0 or (DATEDIFF(D, convert(date, @departureday, 103), GETDATE()) = 0  and convert(time(0), GETDATE()) >= (select ti.STARTTIME from TBL_TIMEBUSLINE ti where IDTIME = @idTime)))
	begin
		set @strResult = N'Không thể thêm chuyến đi thời gian trước thời gian hiện tại !!!'
		return
	end
	if not exists (select * from TBL_EMPLOYEE e, TBL_TYPEOFEMPLOYEE te where e.IDEMPLOYEE = @idEmployee and e.IDTYPE = te.IDTYPE and te.NAMETYPE like N'%Lơ xe%')
	begin
		set @strResult = N'Nhân viên này không thể theo xe !!!'
		return
	end

	if not exists (select * from TBL_DRIVER where IDDRIVER = @idDriver)
	begin
		set @strResult = N'Mã tài xế không tồn tại !!!'
		return
	end
		
	if exists (select * from TBL_TRIP where IDTIME = @idTime and IDBUSLINE = @idBusLine and DEPARTUREDAY = convert(date,@departureday,103))
	begin
		set @strResult = N'Chuyến đi đã tồn tại !!!'
		return
	end
		 
	if exists (select * from TBL_TRIP where DEPARTUREDAY = convert(date,@departureday,103) and IDEMPLOYEE = @idEmployee and IDTIME = @idTime)
	begin
		set @strResult = N'Nhân viên này đã được phân công !!!'
		return
	end
		
	if exists (select * from TBL_TRIP where DEPARTUREDAY = convert(date,@departureday,103) and IDDRIVER = @idDriver and IDTIME = @idTime)
	begin
		set @strResult = N'Tài xế này đã được phân công !!!'
		return
	end
		
	if exists (select * from TBL_TRIP where DEPARTUREDAY = convert(date,@departureday,103) and IDCOACH = @idCoach and IDTIME = @idTime)
	begin
		set @strResult = N'Xe này đã có lịch !!!'
		return
	end
		
	insert into TBL_TRIP(IDTIME, IDBUSLINE, IDEMPLOYEE, IDCOACH, IDDRIVER, DEPARTUREDAY) values (@idTime, @idBusLine, @idEmployee, @idCoach, @idDriver, convert(date,@departureday,103))
	set @strResult = N'Thành công.'
GO

CREATE PROC sp_GetIDPriceTicket
AS
	declare @id int
	select @id = IDPRICE from TBL_PRICE where EFFECTIVEDATE = (select max(EFFECTIVEDATE) from TBL_PRICE)
	return @id
GO


CREATE PROC sp_InsertAndGetIdBill @idEmployee int, @idClient int, @idBill int output, @strResult nvarchar(max) output
AS
	set @idBill = 0
	if not exists (select * from TBL_EMPLOYEE e, TBL_PERMISSIONGROUP p where IDEMPLOYEE = @idEmployee and
		e.IDPERMISSIONGROUP = p.IDPERMISSIONGROUP and (p.NAMEGROUP like '%Admin%' or p.NAMEGROUP like '%Bán vé%'))
	begin
		set @strResult = N'Nhân viên này không thể bán vé !!!'
		return
	end
	if not exists (select * from  TBL_CLIENT where IDCLIENT = @idClient)
	begin
		set @strResult = N'Thông tin khách hàng không hợp lệ !!!'
		return
	end
	insert into TBL_BILL(IDEMPLOYEE, IDCLIENT, INVOICEDATE) values (@idEmployee, @idClient, GETDATE())
	set @strResult = N'Thành công'
	set @idBill = (select IDBILL from TBL_BILL where IDBILL = (select max(IDBILL) from TBL_BILL where IDEMPLOYEE = @idEmployee and IDCLIENT = @idClient))
GO

CREATE PROC sp_InsertTicKet @idSeat int, @idTrip int, @idPicUpPoint int, @idDropOffPoint int, @idTicket int output, @strResult nvarchar(max) output
AS
	set @idTicket = 0
	declare @dateOrder datetime = getdate()
	if not exists (select * from TBL_TRIP where IDTRIP = @idTrip)
	begin
		set @strResult = N'Không tìm thấy chuyến đi phù hợp !!!'
		return
	end
	if (@idPicUpPoint = @idDropOffPoint)
	begin
		set @strResult = N'Điểm đón và điểm xuống xe bị trùng nhau !!!'
		return
	end
	if exists (select * from TBL_TRIP where (IDTRIP = @idTrip and DATEDIFF(D, @dateOrder, DEPARTUREDAY) < 0)
		or (DATEDIFF(D, convert(date, @dateOrder, 103), convert(date, DEPARTUREDAY, 103)) = 0  and convert(time(0), @dateOrder) >= (select ti.STARTTIME from TBL_TIMEBUSLINE ti, TBL_TRIP t where ti.IDTIME = t.IDTIME and IDTRIP = @idTrip)))
	begin
		set @strResult = N'Chuyến đi đã quá hạn !!!'
		return
	end
	if exists (select * from TBL_BOOKED where IDTRIP = @idTrip and IDSEAT = @idSeat)
	begin
		set @strResult = N'Ghế đã có người đặt !!!'
		return
	end

	if not exists (select * from TBL_TRIP t, TBL_PICKUP p where t.IDTRIP = @idTrip and t.IDBUSLINE = p.IDBUSLINE and p.IDSTATION = @idPicUpPoint)
	begin
		set @strResult = N'Điểm đón không tồn tại trong tuyến !!!'
		return
	end

	if not exists (select * from TBL_TRIP t, TBL_DROPOFF d where t.IDTRIP = @idTrip and t.IDBUSLINE = d.IDBUSLINE and d.IDSTATION = @idDropOffPoint)
	begin
		set @strResult = N'Điểm xuống xe không tồn tại trong tuyến !!!'
		return
	end
	declare @idPrice int, @pickup nvarchar(max), @dropoff nvarchar(max)
	exec @idPrice = sp_GetIDPriceTicket
	set @pickup = (select NAMESTATION from TBL_STATION where IDSTATION = @idPicUpPoint)
	set @dropoff = (select NAMESTATION from TBL_STATION where IDSTATION = @idDropOffPoint)
	insert into TBL_TICKET(IDSEAT, IDTRIP, IDPRICE, PICKUPPOINT, DROPOFFPOINT) values (@idSeat, @idTrip, @idPrice, @pickup, @dropoff)
	insert into TBL_BOOKED(IDTRIP, IDSEAT) values(@idTrip, @idSeat)
	set @strResult = N'Thành công.'
	set @idTicket = (select IDTICKET from TBL_TICKET where IDTRIP = @idTrip and IDSEAT = @idSeat)
GO

CREATE PROC sp_DeteleBill @idBill int
AS
	if not exists (select * from TBL_BILL where IDBILL = @idBill)
		return
	if not exists (select * from TBL_BILLDETAILS where IDBILL = @idBill)
	begin
		delete TBL_BILL
		where IDBILL = @idBill
	end
GO

CREATE PROC sp_DeteleTicket @idTicket int
AS
	if not exists (select * from TBL_TICKET where IDTICKET = @idTicket)
		return
	if not exists (select * from TBL_BILLDETAILS where IDTICKET = @idTicket)
	begin
		delete TBL_TICKET
		where IDTICKET = @idTicket
	end
GO

CREATE PROC sp_InsertBillDetails @idBill int, @idTicket int, @strResult nvarchar(max) output
AS
	if not exists (select * from TBL_BILL where IDBILL = @idBill)
	begin
		set @strResult = N'Hóa đơn không tồn tại !!!'
		return
	end
	if not exists (select * from TBL_TICKET where IDTICKET = @idTicket)
	begin
		set @strResult = N'Vé không tồn tại !!!'
		return
	end
	insert into TBL_BILLDETAILS(IDBILL, IDTICKET) values(@idBill, @idTicket)
	set @strResult = N'Thành công.'
GO

CREATE PROC sp_GetIDClient @phone varchar(max)
AS
	declare @id int
	if not exists (select * from TBL_CLIENT where PHONECLIENT = @phone)
		return 0
	select @id = IDCLIENT from TBL_CLIENT where PHONECLIENT = @phone
	return @id
GO

CREATE PROC sp_DeleteAccount @idAccount int, @result nvarchar(max) output
AS
	set @result = N'Thành công.'
	if exists (select * from TBL_EMPLOYEE where IDACCOUNT = @idAccount)
	begin
		if exists (select * from TBL_EMPLOYEE e, TBL_ACCOUNT a, TBL_PERMISSIONGROUP p where e.IDACCOUNT = a.IDACCOUNT and e.IDPERMISSIONGROUP = p.IDPERMISSIONGROUP and p.NAMEGROUP != N'Không')
		begin
			set @result = N'Tài khoản thuộc sở hữu của một nhân viên không thể xóa !!!'
			return
		end
		update TBL_EMPLOYEE
		set IDACCOUNT = null
		where IDEMPLOYEE = (select IDEMPLOYEE from TBL_EMPLOYEE where IDACCOUNT = @idAccount)
	end
	delete TBL_ACCOUNT where IDACCOUNT = @idAccount
GO

CREATE PROC sp_InsertAccount @idEmployee int, @userName varchar(max), @typeAccount nvarchar(max), @strResult nvarchar(max) output
AS
	set @strResult = N'Thất bại !!!'
	if exists (select * from TBL_EMPLOYEE where IDEMPLOYEE = @idEmployee)
	begin
		if not exists (select * from TBL_ACCOUNT where USERNAME = @userName)
		begin
			if exists (select * from TBL_PERMISSIONGROUP pg where pg.NAMEGROUP = @typeAccount)
			begin
				declare @idPG int
				set @idPG = (select IDPERMISSIONGROUP from TBL_PERMISSIONGROUP pg where pg.NAMEGROUP = @typeAccount)
				insert into TBL_ACCOUNT (IDEMPLOYEE, USERNAME) values (@idEmployee, @userName)
				declare @idAccount int
				set @idAccount = (select IDACCOUNT from TBL_ACCOUNT where USERNAME = @userName)
				update TBL_EMPLOYEE set IDACCOUNT = @idAccount, IDPERMISSIONGROUP = @idPG where IDEMPLOYEE = @idEmployee
				set @strResult = N'Thành công.'
			end
		end
	end
GO

CREATE PROC sp_UpdateAccount @idEmployee int, @idAccount int, @userName varchar(max), @typeAccount nvarchar(max), @strResult nvarchar(max) output
AS
	set @strResult = N'Thất bại !!!'
	if exists (select * from TBL_EMPLOYEE where IDEMPLOYEE = @idEmployee)
	begin
		if exists (select * from TBL_ACCOUNT where IDACCOUNT = @idAccount)
		begin
			if exists (select * from TBL_PERMISSIONGROUP pg where pg.NAMEGROUP = @typeAccount)
			begin
				declare @idPG int
				set @idPG = (select IDPERMISSIONGROUP from TBL_PERMISSIONGROUP pg where pg.NAMEGROUP = @typeAccount)
				update TBL_ACCOUNT set USERNAME = @userName where IDACCOUNT = @idAccount
				update TBL_EMPLOYEE set IDPERMISSIONGROUP = @idPG where IDEMPLOYEE = @idEmployee
				set @strResult = N'Thành công.'
			end
		end
	end
GO

CREATE PROC sp_GetIDBill @idTicket int
AS
BEGIN
	declare @id int = 0
	if exists (select * from TBL_BILLDETAILS where IDTICKET = @idTicket)
		set @id = (select b.IDBILL from TBL_BILL b, TBL_BILLDETAILS bd where b.IDBILL = bd.IDBILL and bd.IDTICKET = @idTicket)
	return @id
END
GO

CREATE PROC sp_DeleteTicket @idTicket int, @result nvarchar(max) output
AS
BEGIN
	set @result = N'Không thể hủy vé !!!'
	if exists (select * from TBL_TICKET where IDTICKET = @idTicket)
	begin
		declare @date datetime = GETDATE()
		if exists(select * from TBL_TRIP t, TBL_TICKET tic, TBL_TIMEBUSLINE ti where tic.IDTICKET = @idTicket and tic.IDTRIP = t.IDTRIP and t.IDTIME = ti.IDTIME and (DATEDIFF(D, @date, DEPARTUREDAY) < 0)
			or (DATEDIFF(D, convert(date, @date, 103), convert(date, DEPARTUREDAY, 103)) = 0  and convert(time(0), @date) >= ti.STARTTIME))
		begin
			return
		end
		declare @idBill int
		if exists (select * from TBL_BILLDETAILS where IDTICKET = @idTicket)
		begin
			select @idBill = IDBILL from TBL_BILLDETAILS where IDTICKET = @idTicket
			delete TBL_BILLDETAILS where IDTICKET = @idTicket
			exec sp_DeteleBill @idBill
			delete TBL_BOOKED where IDTRIP = (select tic.IDTRIP from TBL_TICKET tic where tic.IDTICKET = @idTicket) and IDSEAT = (select tic.IDSEAT from TBL_TICKET tic where tic.IDTICKET = @idTicket)
			delete TBL_TICKET where IDTICKET = @idTicket
			set @result = N'Thành công.'
			return
		end
	end
END
GO

------------------------------------------------------------------------------- HOÀNG THỊ MỸ HẠNH
------------------DRIVER-----------------------
-----------------------update driver----------------------
CREATE PROC sp_UpdateDriver @IDDRIVER int,
							@IDWARD int,
							@NAMEDRIVER nvarchar(MAX),
							@DATEOFBIRTHDRIVER varchar(max),
							@GENDERDRIVER nvarchar(MAX), 
							@IDENTITYCARDDRIVER varchar(12),
							@PHONEDRIVER varchar(10),
							@EMAILDRIVER varchar(200)			
AS 
	BEGIN 
		
		UPDATE TBL_DRIVER
			SET NAMEDRIVER = @NAMEDRIVER , IDWARD = @IDWARD, DATEOFBIRTHDRIVER  = convert(date, @DATEOFBIRTHDRIVER),
			GENDERDRIVER = @GENDERDRIVER, IDENTITYCARDDRIVER = @IDENTITYCARDDRIVER, PHONEDRIVER  =@PHONEDRIVER,
			EMAILDRIVER = @EMAILDRIVER, DEGREE = 'E' 
		WHERE IDDRIVER = @IDDRIVER 
		
	END
GO
-------------------delete driver-----------------------------------------
CREATE PROC sp_deleteDriver @idDriver int
AS
	if not exists (select * from TBL_TRIP where IDDRIVER = @idDriver)
	begin
		--update TBL_TRIP
		--set	IDDRIVER = null
		--where IDDRIVER = @idDriver
		delete TBL_DRIVER where IDDRIVER = @idDriver
	end
GO
--------------------------insert driver-------------------------------
CREATE PROC sp_InsertDriver @IDWARD int, 
							@NAME nvarchar(max),
						    @DATEOFBIRTH date, 
							@GENDER nvarchar(4), 
							@IDENTITYCARD varchar(12),
							@PHONE varchar(10),
							@EMAIL varchar(200)	
AS 
	BEGIN 
		INSERT INTO TBL_DRIVER(IDWARD, NAMEDRIVER, DATEOFBIRTHDRIVER, GENDERDRIVER, IDENTITYCARDDRIVER,PHONEDRIVER, EMAILDRIVER)
		VALUES (@IDWARD, @NAME, @DATEOFBIRTH, @GENDER, @IDENTITYCARD, @PHONE, @EMAIL)
	END
GO
------------------------------------------------------------------------------------------------------------------ 		

------------------------------------------------------------------------------------------------------------------ MAI TRUNG TIẾN

CREATE PROC sp_UpdateEMPLOYEE @IDEMPLOYEE int,
							@IDWARD int,
							@IDTYPE int,
							@NAMEEMPLOYEE nvarchar(max),
							@DATEOFBIRTHEMPLOYEE varchar(max), 
							@GENDEREMPLOYEE nvarchar(4),
							@IDENTITYCARDEMPLOYEE varchar(12),
							@PHONEEMPLOYEE varchar(10),
							@EMAILEMPLOYEE varchar(200)					
AS 
	BEGIN 
		UPDATE TBL_EMPLOYEE 
			SET NAMEEMPLOYEE = @NAMEEMPLOYEE , IDWARD = @IDWARD, DATEOFBIRTHEMPLOYEE = @DATEOFBIRTHEMPLOYEE ,
			GENDEREMPLOYEE = @GENDEREMPLOYEE, IDENTITYCARDEMPLOYEE = @IDENTITYCARDEMPLOYEE, PHONEEMPLOYEE =@PHONEEMPLOYEE,
			EMAILEMPLOYEE = @EMAILEMPLOYEE, IDTYPE = @IDTYPE 
		WHERE IDEMPLOYEE = @IDEMPLOYEE 
		
	END
go


CREATE PROC sp_DeleteEMPLOYEE @IDEMPLOYEE int							
AS 
	BEGIN 
		
		--
		IF  EXISTS (SELECT * FROM TBL_BILL WHERE @IDEMPLOYEE = IDEMPLOYEE )
		BEGIN  
			DELETE FROM TBL_EMPLOYEE WHERE  IDEMPLOYEE= @IDEMPLOYEE 
			RETURN
		END
		IF EXISTS (SELECT * FROM TBL_TRIP WHERE @IDEMPLOYEE = IDEMPLOYEE)
		BEGIN 
			DELETE FROM TBL_EMPLOYEE WHERE  IDEMPLOYEE= @IDEMPLOYEE 
			RETURN
		END
		IF EXISTS (SELECT * FROM TBL_SERVICE WHERE @IDEMPLOYEE = IDEMPLOYEE)
		BEGIN
			DELETE FROM TBL_EMPLOYEE WHERE  IDEMPLOYEE= @IDEMPLOYEE
			RETURN
		END
		--
		IF NOT EXISTS(SELECT * FROM TBL_ACCOUNT WHERE IDEMPLOYEE = @IDEMPLOYEE)
			BEGIN 
				IF EXISTS(SELECT * FROM TBL_EMPLOYEE WHERE IDEMPLOYEE = @IDEMPLOYEE)
				DELETE FROM TBL_EMPLOYEE WHERE @IDEMPLOYEE = IDEMPLOYEE
				RETURN
			END
		
		IF EXISTS(SELECT * FROM TBL_ACCOUNT WHERE IDEMPLOYEE = @IDEMPLOYEE)
		BEGIN 
			IF EXISTS(SELECT * FROM TBL_EMPLOYEE WHERE IDEMPLOYEE = @IDEMPLOYEE)
			BEGIN
				DECLARE @IDACCOUNT INT
				SET @IDACCOUNT = (SELECT IDACCOUNT FROM TBL_ACCOUNT  WHERE IDEMPLOYEE = @IDEMPLOYEE)
				UPDATE TBL_EMPLOYEE 
				SET IDACCOUNT = NULL
				WHERE IDEMPLOYEE = (SELECT IDEMPLOYEE FROM TBL_EMPLOYEE WHERE  IDACCOUNT = @IDACCOUNT )
				DELETE FROM  TBL_ACCOUNT WHERE IDEMPLOYEE = @IDEMPLOYEE
				DELETE FROM TBL_EMPLOYEE WHERE  IDEMPLOYEE= @IDEMPLOYEE
				RETURN
			END
		END
		
	END
GO

CREATE PROC sp_InsertEMPLOYEE @IDWARD int, @IDTYPE int,
							@NAMEEMPLOYEE nvarchar(max), @DATEOFBIRTHEMPLOYEE varchar(100), 
							@GENDEREMPLOYEE nvarchar(4), @IDENTITYCARDEMPLOYEE varchar(12),
							@PHONEEMPLOYEE varchar(10),@EMAILEMPLOYEE varchar(200)	

AS 
	BEGIN 
		INSERT INTO TBL_EMPLOYEE (IDWARD,IDTYPE,NAMEEMPLOYEE,DATEOFBIRTHEMPLOYEE,
									GENDEREMPLOYEE,IDENTITYCARDEMPLOYEE,PHONEEMPLOYEE,EMAILEMPLOYEE)
			VALUES (@IDWARD, @IDTYPE, @NAMEEMPLOYEE, @DATEOFBIRTHEMPLOYEE, 
							@GENDEREMPLOYEE, @IDENTITYCARDEMPLOYEE,@PHONEEMPLOYEE, @EMAILEMPLOYEE)
	END
GO

------------------------------------------------------------------------------------------------------------------ End MAI TRUNG TIẾN

------------------------------------------------------------------------------------------------------------------ TRẦN MỸ HẰNG
CREATE PROC sp_GetTimeBusLine @IDTRIP INT,@STARTTIME TIME(0) OUTPUT, @FINISHTIME TIME(0) OUTPUT
AS
BEGIN 
	SELECT @STARTTIME = STARTTIME, @FINISHTIME = FINISHTIME
	FROM TBL_TIMEBUSLINE,TBL_TRIP
	WHERE IDTRIP = @IDTRIP AND TBL_TIMEBUSLINE.IDTIME = TBL_TRIP.IDTIME
END
GO

CREATE PROC sp_GetBusLine @IDTRIP INT, @DEPARTURESTATION nvarchar(MAX) OUTPUT, @DESTINATIONSTATION nvarchar(MAX) OUTPUT
AS
BEGIN 
	SELECT @DEPARTURESTATION = DEPARTURESTATION, @DESTINATIONSTATION = DESTINATIONSTATION
	FROM TBL_BUSLINE, TBL_TRIP
	WHERE IDTRIP = @IDTRIP AND TBL_BUSLINE.IDBUSLINE = TBL_TRIP.IDBUSLINE
END
GO

CREATE PROC sp_DeleteTrip @IDTRIP INT,@RESULT INT OUTPUT
AS
	IF NOT EXISTS(SELECT * FROM TBL_TRIP WHERE IDTRIP = @IDTRIP)
		RETURN
	IF NOT EXISTS(SELECT * FROM TBL_SERVICE WHERE IDTRIP = @IDTRIP)
	BEGIN
		IF NOT EXISTS(SELECT * FROM TBL_BOOKED WHERE IDTRIP = @IDTRIP)
		BEGIN
			IF NOT EXISTS(SELECT * FROM TBL_TICKET WHERE IDTRIP = @IDTRIP)
			BEGIN 
				DELETE TBL_TRIP
				WHERE IDTRIP = @IDTRIP
				SET @RESULT = 1
			END
		END
	END
GO

------------------------------------------------------------------------------------------------------------------ End TRẦN MỸ HẰNG

CREATE PROCEDURE sp_Change_Password
(             
                @username  VARCHAR(100),
                @old_pwd   VARCHAR(50),
                @new_pwd   VARCHAR(50),
                @status    int OUTPUT
)
AS
BEGIN             
	IF EXISTS(SELECT * FROM TBL_ACCOUNT WHERE USERNAME=@username AND PASSWORD=@old_pwd)
		  BEGIN
			   UPDATE TBL_ACCOUNT SET PASSWORD=@new_pwd WHERE UserName=@username
			   SET @status=1
		  END
	ELSE                    
		  BEGIN 
			   SET @status=0
		  END     
END

/*==========================================================================================================================*/
/*                                                          FUNCTION                                                        */
/*==========================================================================================================================*/
CREATE FUNCTION f_GetTripByIDBusLineDepartureday(@idBusline int, @departureday datetime)
RETURNS TABLE
AS
	return (select t.IDTRIP, cast(ti.STARTTIME as nvarchar(max)) + ' - ' + cast(ti.FINISHTIME as nvarchar(max)) as 'Time', e.NAMEEMPLOYEE, c.LICENSEPLATE, d.NAMEDRIVER, t.DEPARTUREDAY, t.AMOUNTEMPTYSEAT, (select top(1) PRICE from TBL_PRICE where EFFECTIVEDATE = (select Max(EFFECTIVEDATE) from TBL_PRICE)) as 'Price'
	from TBL_TRIP t, TBL_TIMEBUSLINE ti, TBL_BUSLINE b, TBL_EMPLOYEE e, TBL_COACH c, TBL_DRIVER d
	where b.IDBUSLINE = @idBusline and DATEDIFF(D, t.DEPARTUREDAY, @departureday) = 0 and ti.IDTIME = t.IDTIME and ti.STARTTIME > cast(GETDATE() as time(0)) and b.IDBUSLINE = t.IDBUSLINE and e.IDEMPLOYEE = t.IDEMPLOYEE and c.IDCOACH = t.IDCOACH and d.IDDRIVER = t.IDDRIVER)
GO

CREATE FUNCTION f_GetSeatByIDTrip(@idTrip int)
RETURNS TABLE
AS
	return (select s.SEATPOSITION
			from TBL_BOOKED b, TBL_SEAT s
			where b.IDTRIP = @idTrip and b.IDSEAT = s.IDSEAT)
GO

CREATE FUNCTION f_GetBills()
RETURNS TABLE
AS
	return (select b.IDBILL, e.NAMEEMPLOYEE, c.NAMECLIENT, convert(date, c.DATEOFBIRTHCLIENT, 103) AS 'DATEOFBIRTHCLIENT', b.TOTALMONEY, convert(date, b.INVOICEDATE, 103) AS 'INVOICEDATE' from TBL_BILL b, TBL_EMPLOYEE e, TBL_CLIENT c where b.IDEMPLOYEE = e.IDEMPLOYEE and b.IDCLIENT = c.IDCLIENT)
GO

CREATE FUNCTION f_GetBillDetails(@idBill int)
RETURNS TABLE
AS
	return (select tic.IDTICKET, bus.DEPARTURESTATION + ' - ' + bus.DESTINATIONSTATION as 'BUSLINE', cast(ti.STARTTIME as nvarchar(max)) + ' - ' + cast(ti.FINISHTIME as nvarchar(max)) as 'Time',
				t.DEPARTUREDAY, c.LICENSEPLATE, s.SEATPOSITION, tic.PICKUPPOINT, tic.DROPOFFPOINT, p.PRICE
			from TBL_BILLDETAILS b, TBL_TICKET tic, TBL_SEAT s, TBL_TRIP t, TBL_PRICE p, TBL_BUSLINE bus, TBL_COACH c, TBL_TIMEBUSLINE ti
			where b.IDBILL = @idBill and tic.IDTICKET = b.IDTICKET and tic.IDSEAT = s.IDSEAT and tic.IDTRIP = t.IDTRIP and tic.IDPRICE = p.IDPRICE and
			t.IDBUSLINE = bus.IDBUSLINE and t.IDCOACH = c.IDCOACH and t.IDTIME = ti.IDTIME)
GO


CREATE FUNCTION f_GetBusLine(@DEPARTURESTATION nvarchar(max),@DESTINATIONSTATION nvarchar(max))
RETURNS TABLE
AS
	RETURN (SELECT IDBUSLINE 
	FROM TBL_BUSLINE 
	WHERE DEPARTURESTATION = @DEPARTURESTATION AND DESTINATIONSTATION = @DESTINATIONSTATION)
GO

create function f_GetEmployee(@nameEmployee nvarchar(max))
RETURNS TABLE
AS 
	RETURN(SELECT IDEMPLOYEE FROM TBL_EMPLOYEE WHERE NAMEEMPLOYEE = @nameEmployee)
GO

create function f_GetTimeBusLine(@start time(0),@finish time(0))
RETURNS TABLE 
AS
	RETURN(SELECT IDTIME FROM TBL_TIMEBUSLINE WHERE STARTTIME = @start AND FINISHTIME = @finish)
GO

create function f_GetCoach(@LICENSEPLATE varchar(20))
returns table 
as
	return(select IDCOACH from TBL_COACH where LICENSEPLATE = @LICENSEPLATE)
go

create function f_GetDriver(@nameDriver nvarchar(MAX))
returns table
as
	return(select IDDRIVER from TBL_DRIVER where NAMEDRIVER = @nameDriver)
go