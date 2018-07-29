package tke.mobility.support;


import java.sql.Connection;
import java.sql.DriverManager;
import java.sql.ResultSet;
import java.sql.SQLException;
import java.sql.Statement;
import oracle.jdbc.driver.*;

/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */

/**
 *
 * @author Rich
 */
public class MyDBConnection {
     ResultSet outResults = null;
    public  ResultSet connect(String qry,String DbConnect)throws ClassNotFoundException, SQLException {
        
          try{
                //Class.forName("org.apache.derby.jdbc.ClientDriver"); 
                //String url = "jdbc:derby://localhost:1527/sample";
                Class.forName("oracle.jdbc.OracleDriver");
                String usr = ""; 
                String pwd = "";
                String url = "";
                
                if (DbConnect == "ANT"){             
                    url = StringConstants.ANTConnection;
                    usr =  "ANTCFG";
                    pwd = "antcfg";
                }
                if (DbConnect == "OOD"){             
                    url = StringConstants.OODConnection;
                    usr =  "rac_accnt";
                    pwd = "QtXhRMB5";
                }
                if (DbConnect == "SVG"){
                       url = StringConstants.SVGConnection;
                       usr ="SVG";
                       pwd ="SVG";
                }
                if (DbConnect == "ANTINTG"){
                       url = StringConstants.ANTINTGConnection;
                       usr ="ANTINTG";
                       pwd ="ANTINTG";
                }
                Connection conn = DriverManager.getConnection(url,usr,pwd);         
                Statement stmt  = conn.createStatement();
                if (qry!= null)
                {
                    String sql = qry;   
                    ResultSet rs =stmt.executeQuery(sql);
                    outResults = rs;
                }
               
          } 
          
           catch (ClassNotFoundException ex){
         //catch (Exception ex){
                System.out.println("Unable to connect to database."+ ex);
        
          }
    
        return outResults;
        
    }
    
}
