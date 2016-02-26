﻿# ContactCheckupSync
### โปรแกรมซิงค์ข้อมูลระหว่างระบบ ContactCheckup และ การออกหน่วยแบบ Mobile
![alt tag](https://github.com/oofdui/ContactCheckupSync/blob/master/SS.png)

* v.1.22
	* แก้หน้ารีพอร์ต ส่วนการแสดงรายการตรวจที่ยกเลิก
* v.1.21
	* แก้ปัญหาการแสดงข้อความค้างคืนเอกสารด้วยเงื่อนไขใหม่
* v.1.20
	* ปรับฟิลด์ดึง RegisterDate จาก MWhen เป็น RegDate
	* เพิ่มตัวกรอง กรณีมีวันที่ลงทะเบียนแต่ยังตรวจไม่ครบเป็น "ค้างคืนเอกสาร"
* v.1.19
	* แก้ปัญหาชื่อ BookCreate เป็น Null ทำให้ตอน Export ไฟล์ Excel โปรแกรมฟ้องว่าสร้าง Sheet ชื่อซ้ำกัน
* v.1.18
	* แก้รีพอร์ต Excel ให้เพิ่มตัวเลือกว่าจะ เอา/ไม่เอา Checklist ที่เป็นโลหะหนัก
* v.1.16
	* เพิ่มฟิลด์ BookCreate ในตัวช่วยสร้างตาราง
	* เพิ่มเทเบิ้ล staff,log_print ในตัวช่วยสร้างตาราง
	* ซิงค์ BookCreate มาด้วย
	* ปรับขั้นตอนการ SyncToMobile ใหม่ โดยให้เช็คสถานะ Checklist ด้วย ถ้ามีการอัพเดทฝั่ง Main ให้วนมาอัพเดทที่ Mobile ด้วย (ProStatus@Main > ProStatus@Mobile)
	* เพิ่มการซิงค์ Checklist และ ChecklistDetail ในการ SyncToMobile โดยทำการลบข้อมูลเดิมฝั่ง Mobile ก่อนแล้วซิงค์เข้าไปใหม่ เพื่อกันการอัพเดทฝั่งโรงพยาบาล
	* คอลัมภ์ในไฟล์ที่ Export ออกมา กรณีเป็นวันที่ ในไฟล์ต้องเป็นวันที่ด้วย เพื่อให้สามารถใช้ฟังชันกรองของ Excel ได้แบบแยกวัน เดือน ปี
	* แยก Sheet ไฟล์ที่ Export ได้แล้ว ทั้งแบบเลือก All , Payor , Book โดยแยกได้ที่ Sheet Detail , Sheet Lab
	* เพิ่ม Sheet Summary
* v.1.15
	* โมดูล SyncToMobile หลังจากกดค้นหา ระบบจะไฮไลท์คนไข้ที่ยังไม่มี Checklist ด้วยสีแดง และ แสดงจำนวนด้านบน
* v.1.14
	* ย้ายเมนู SyncToMain ไว้ต่างหาก และ ให้โปรแกรมทำงานอยู่เบื้องหลัง แม้ว่าจะสลับหน้าไปทำงานโมดูลอื่นก็ตาม พร้อมกับเปลี่ยนสถานะบนแทปเพื่อบอกการทำงานของโมดูล SyncToMain ได้
* v.1.13
	* แก้ปัญหากรณีต่อฐานข้อมูลแม่ไม่ได้ ให้สร้างไฟล์ด้วย
* v.1.12
	* ปรับหน้ารายงาน Sheet Detail ให้ดึงข้อความที่พิมพ์เมื่อมีการยกเลิกการตรวจมาแสดงแทนการดึงว่า ยกเลิกอะไรไปบ้าง
* v.1.9
	* เพิ่มชื่อ ComputerName ในไฟล์ที่ใช้ Sync
* v.1.8
	* เพิ่มตัวเลือกช่วงเวลาย้อนหลังที่เปิดให้โปรแกรมดึงข้อมูลที่มีการเปลี่ยนแปลงย้อนหลังตามเวลาที่กำหนด
	* เพิ่มตัวแสดงพาร์ธที่เก็บไฟล์ทั้งฝั่ง Main และ Mobile พร้อมปุ่มสำหรับคลิกเพื่อเปิดโฟล์เดอร์
	* เพิ่มตัวแสดงจำนวน Rows ที่ระบบสร้างไฟล์ Sync
	* ปรับรีพอร์ต กรณียกเลิกการตรวจให้นับเป็นตรวจเสร็จแล้ว
* v.1.6
	* AutoStart โหมด SyncToMain เมื่อเปิดโปรแกรม
	* เพิ่มเงื่อนไขในการดึงข้อมูล PatientChecklist ให้เอาเฉพาะข้อมูลที่มีการเปลี่ยนแปลง (MWhen) ภายใน 3 ชั่วโมง
* v.1.5
	* ปรับโหมด SyncToMain เป็นแบบสร้างไฟล์แล้วอัพโหลดมาไว้บนเซิฟเวอร์ตามพาร์ธที่กำหนด
* v.1.4
	* เพิ่มการอัพเดทข้อมูลในฟิลด์ Patient.StatusOnMobile
* v.1.0
  * เริ่มสร้าง
