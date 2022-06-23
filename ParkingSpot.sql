DROP TABLE IF EXISTS "ParkingSpot";

CREATE TABLE "ParkingSpot" (
	"ID" INTEGER PRIMARY KEY,
	"LicensePlate" nvarchar (20) NOT NULL ,
	"Ocuppied" "bit" NOT NULL DEFAULT(0),
	"DateEntered" "datetime" NULL ,
	"DepartureDate" "datetime" NULL ,
	"Rate" "money" NULL
);

INSERT INTO "ParkingSpot"("ID","LicensePlate","Ocuppied","DateEntered","DepartureDate","Rate") VALUES(1,'PDH1695',1,'20220622 9:52:00 AM','20220622 11:15:00 AM',3.5);
INSERT INTO "ParkingSpot"("ID","LicensePlate","Ocuppied","DateEntered","DepartureDate","Rate") VALUES(2,'',0,'','',3.5);
INSERT INTO "ParkingSpot"("ID","LicensePlate","Ocuppied","DateEntered","DepartureDate","Rate") VALUES(3,'ABC1234',1,'20220622 9:54:00 AM','',3.5);