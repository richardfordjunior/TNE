/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */
package tke.mobility.support;

/**
 *
 * @author
 */ 

public interface StringConstants {
    public static String reportNames ="Choose Report; <-------EBS------->;Debriefs sent from Mobile;Repair/Construction Tickets Posted;Repair/Construction ToolBox Talks tickets posted;<-------SVG------->;Tickets sent to Mobile from SVG;Ticket Status (BC);<-------ANT------->;AMP Device Connectivity";
    public static String RepairToolBoxTicketsPosted ="SELECT mec.TRANSACTION_ID,MEC.ATTRIBUTE1,MEC.ATTRIBUTE2,MEC.ATTRIBUTE3,MEC.ATTRIBUTE4,mec.PROJECT_ID" +
                                                                                ",mec.PERSON_ID,mec.COST_CENTER_ID ,mec.CREATION_DATE ,mec.DEVICE_ID ,mec.EMPLOYEE_NUMBER,mec.FIRST_NAME  ,mec.LAST_NAME"+
                                                                                ",mec.PROCESSED ,mec.ORGANIZATION_ID ,mec.POST_TO_PDA,mec.FORCED_UNSCHEDULE FROM bolinf.tke_tp_cus_xd_05_co_mec_aud mec   WHERE mec.post_to_pda = 'Y'" ;
    public static String MechanicDebriefs ="Select creation_date,entry_source ,source,line_type,expenditure_type,service_activity_code,project_id,proj_desc,description,task_id,task,task_name,task_desc" +
                            ",incident_task_id,week_ending_date,activity_date,start_time,end_time,duration,quantity,uom_code,amount,attribute9" +
                            ",attribute14,attribute15 as DeviceId,task_status,serial_number,building_name,nick_name,item_description,status_ap,status_pa,status_pay,status_cs  from apps.tke_tp_cus_cd_14_lines_v  where 1 =1  ";
    public static String TicketsSentToDeviceFromSVG ="select to_char(w.createddt,'dd-MON-yy HH:MM')  as CreatedDate, imp.updateddt as LastUpdated,w.id as OracleId, prj.projectName as DispatchArea, ip.installsitename as InstallSite " +
                                                                                     "from imp_dex_tix imp, svc_work_orders w, ipcs_install_site ip , svc_projects prj  where 1=1 and imp.workorderhostid = w.id and ip.installsiteid = w.instsite_fk and PRJ.pk = W.PROJECT_FK ";
    public static String PopulateDateCriteria ="Today;7 days ago;2 weeks ago;1 month ago;All;Let me enter dates";
    public static String PopulateEmployees ="select  e.name ||'('|| e.id||':'|| a.emp_tkeroutes ||')' as Employee from ipcs_employees e join ipcs_employee_attrs a on A.EMPLOYEEID = e.pk where E.ACTIVE ='y' and e.employee_type_fk  <> 5 order by 1";
    public static String TicketsPostedForRepCon = "SELECT   req.TRANSACTION_ID,req.TICKET_TYPE,req.TASK_NUMBER,req.TOTAL_DURATION,req.BRANCH_NUMBER,req.SR_NUMBER,req.DISPATCH_AREA,req.ACCESS_NOTE,req.DISPATCH_REQUIRED,req.duration" +
       ",req.CREATION_DATE,req.INSTALL_LOCATION_ID,req.IS_SOUNDNET,req.MECHANIC_EMP_REFERENCE,req.ORG_ID,req.SCHEDULED_START_DATE"+           
       ",req.SCHEDULED_END_DATE,req.STATUS_CODE ,req.STATUS_MESSAGE FROM bolinf.tke_tp_cus_xd_05_sv_req_aud req  WHERE 1=1 AND req.status_code='SUCCESS'";
    public static String AntennaDeviceConnectivity =     
      " Select PersonId,UserName,Branch,ConnectedStatus"+
      ",to_char(ConnectedTime0,'dd-MON-yy HH:MM') as EventConnectedTime"+
       ",to_char(DisConnectedTime,'dd-MON-yy HH:MM') as EventDisconnectedTime"+
       ",Server"+
       ",ConnectedApplication as AppId"+
        ",DeviceId"+
        " FROM"+
        "(select distinct"+
        " s.subscriberId as PersonId"+
        ",s.pkey"+
       ",d.subscriberfk"+
        ",s.FirstName ||' '||s.lastName as UserName"+
         ",grp.description as Branch "+
        ",s.Email"+
        ",s.Status"+
        ",case when ch.state =0 then NULL else ch.EventTime   end as ConnectedTime0"+
        ",ch_1.EventTime as ConnectedTime1"+
        ",(select eventtime from ANTCFG.OWCONNHISTORY"+
        " where 1=1 and state = 0  and subscriberfk = s.pkey and eventtime = ch.eventtime) as DisConnectedTime"+
        ",ch.Server"+
        ",case  when ch.state =1 then 'Online' else 'Offline' end as ConnectedStatus"+
        ",CH.APPID as ConnectedApplication"+
        ",CH.DEVICEID"+
        ",s.groupfk"+
        " from ANTCFG.OWSUBSCRIBER s"+
       ",ANTCFG.OWCONNHISTORY ch"+
        ",ANTCFG.OWCONNHISTORY ch_1"+
        ",ANTCFG.OWDEVICE d"+
        ",ANTCFG.OWGROUP grp"+
        " where 1=1"+
        " and d.deviceid = ch.deviceid"+
        " and d.subscriberfk  = s.pkey"+
        " and ch.pkey = ch_1.pkey"+
       " and grp.pkey = s.groupfk "+
        " and s.status = 'AC' ";

public static String TicketHistory ="select SEQUENCEID, TICKETID, STATUS, SOURCE, to_char(RAWXML) as RAWXML, ODISTATUS, MWSTATUS, "+
                                                        "EBSSTATUS, MESSAGE, PDAPERSONID, DEVICEID, APPID, CREATEDBY, CREATIONDATE," +
                                                        "LASTUPDATEDBY, LASTUPDATEDATE, LASTUPDATELOGIN, REQUESTID, BATCHID, WEBSERVICEID," +
                                                        "RECORD_STATUS, ATTEMPTS from ticketstatus_db where 1=1 ";

public static String  HealthCheckOverBooked = "true";
public static String  HealthCheckBusy = "true";
public static int  HealthCheckRunningUOWNum = 20;
public static int  HealthCheckBusyThreads = 20;
public static String OODConnection = "jdbc:oracle:thin:@AUOHSTHYK10.oracleoutsourcing.com:10010:PTHYKI";
public static String SVGConnection  = "jdbc:oracle:thin:@vmohsthyk006.oracleoutsourcing.com:10010:PTHY7T";
public static String ANTConnection = "jdbc:oracle:thin:@auohsthyk43.oracleoutsourcing.com:10010:PTHYKT1";
public static String ANTINTGConnection ="jdbc:oracle:thin:@auohsthyk43.oracleoutsourcing.com:10010/PTHYKT.ORACLEOUTSOURCING.COM";
}



