/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */
package tke.mobility.support;

import java.sql.ResultSet;
import java.sql.ResultSetMetaData;
import java.sql.SQLException;
import java.text.SimpleDateFormat;
import java.util.Date;
import java.util.Locale;
import java.util.Vector;
import javax.swing.JOptionPane;
import javax.swing.JFrame;
import javax.swing.table.DefaultTableModel;
import java.util.regex.Matcher;
import java.util.regex.Pattern;
import java.util.*;
import javax.mail.*;
import javax.mail.internet.*;
import javax.activation.*;


/**
 *
 * @author Rich Ford
 */
public class Util {
         
    public void ShowErrorMessage(String msg)
    {
        JFrame frame = new JFrame();
        JOptionPane pane = new JOptionPane();
        frame.setVisible(true);
        frame.setAlwaysOnTop(true);
        frame.setLocation(50, 50);
        frame.setSize(200, 200);
        frame.setLocationRelativeTo(null);
        frame.add(pane);
        if(!msg.isEmpty())
        {
            pane.setMessage(msg);        
        }
    }
      public DefaultTableModel BuildTableModel(ResultSet rs) throws SQLException{
      ResultSetMetaData meta = rs.getMetaData();
      //Get columns
      Vector <String> columnName = new Vector<String>();
      int numColumns = meta.getColumnCount();
      for(int column =1;column< numColumns;column++)
      {
          columnName.add(meta.getColumnName(column));
      }
      //Get data
      
      Vector<Vector<Object>> data = new Vector<Vector<Object>>();
      while(rs.next()){
          Vector <Object> vector = new Vector<Object>();
          for(int index =1;index < numColumns;index++){
              vector.add(rs.getObject(index));
          }
          data.add(vector);
      }
      rs.close();
      return new DefaultTableModel(data,columnName);
  }
  
    public static class SaveDates{
        public String Date1;
        public String Date2;
        
        String GetDate1(String d1){
           return  Date1 = d1;
        }
         String GetDate2(String d2){
            return  Date2 = d2;
        }
         
    }
   public static String FormatDate (Date date){
     SimpleDateFormat formatter;
     Locale locale = Locale.getDefault();
     formatter = new SimpleDateFormat("dd-MM-yy",locale);       
     return formatter.format(date);
   }
   public static Boolean ValidateDate(String dateToValidate){
       Boolean retVal = false;
       String pattern = "^[0-9]{1,2}-[a-zA-Z]{3}-[0-9]{3}$";
       retVal = Pattern.matches(pattern, dateToValidate);
       return retVal;
   }
   
   
   public int SendEmail(){
       int success = 0;
                // Recipient's email ID needs to be mentioned.
      String to = "richard.ford@thyssenkrupp.com";

      // Sender's email ID needs to be mentioned
      String from = "richard.ford@thyssenkrupp.com";

      // Assuming you are sending email from localhost
      String host = "localhost";
       //String host = "mdcxch14.na.ops.local";

      // Get system properties
      Properties properties = System.getProperties();

      // Setup mail server
      properties.setProperty("mail.smtp.host", host);
       //properties.setProperty("mdcxch14.na.ops.local", host);
      //properties.setProperty("smtp.mail.thyssenkruppelevator.com",host);
      // Get the default Session object.
      Session session = Session.getDefaultInstance(properties);

      try{
         // Create a default MimeMessage object.
         MimeMessage message = new MimeMessage(session);

         // Set From: header field of the header.
         message.setFrom(new InternetAddress(from));

         // Set To: header field of the header.
         message.addRecipient(Message.RecipientType.TO,
                                  new InternetAddress(to));

         // Set Subject: header field
         message.setSubject("This is the Subject Line!");

         // Now set the actual message
         message.setText("This is actual message");

         // Send message
         Transport.send(message);
        
         
         System.out.println("Sent message successfully...."); 
         success = 1;
      }catch (MessagingException mex) {
          success =0;
         mex.printStackTrace();
      }
      return success;
   }
           }
    

