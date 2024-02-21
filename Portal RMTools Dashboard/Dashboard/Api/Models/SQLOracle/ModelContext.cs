using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Api.Models.SQLOracle;

public partial class ModelContext : DbContext
{
    public ModelContext()
    {
    }

    public ModelContext(DbContextOptions<ModelContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AzRole> AzRoles { get; set; }

//    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
//        => optionsBuilder.UseOracle("DATA SOURCE=127.0.0.1:1440/RMTDBDEV;PERSIST SECURITY INFO=True;USER ID=BNI_OCP_DEV4;Password=BNI_OCP_DEV41;Connection Timeout =7200");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .HasDefaultSchema("BNI_OCP_DEV4")
            .UseCollation("USING_NLS_COMP");

        modelBuilder.Entity<AzRole>(entity =>
        {
            entity.HasKey(e => new { e.Appownerid, e.Applicationid, e.Roleid });

            entity.ToTable("AZ_ROLE");

            entity.Property(e => e.Appownerid)
                .HasPrecision(10)
                .ValueGeneratedOnAdd()
                .HasColumnName("APPOWNERID");
            entity.Property(e => e.Applicationid)
                .HasPrecision(10)
                .ValueGeneratedOnAdd()
                .HasColumnName("APPLICATIONID");
            entity.Property(e => e.Roleid)
                .HasPrecision(10)
                .ValueGeneratedOnAdd()
                .HasColumnName("ROLEID");
            entity.Property(e => e.Applicationtype)
                .HasPrecision(10)
                .HasDefaultValueSql("0")
                .HasColumnName("APPLICATIONTYPE");
            entity.Property(e => e.Description)
                .HasColumnType("NVARCHAR2(2048)")
                .HasColumnName("DESCRIPTION");
            entity.Property(e => e.Ignoresearchscope)
                .IsRequired()
                .HasPrecision(1)
                .HasDefaultValueSql("(0) ")
                .HasColumnName("IGNORESEARCHSCOPE");
            entity.Property(e => e.Ipaddress)
                .HasMaxLength(50)
                .ValueGeneratedOnAdd()
                .HasColumnName("IPADDRESS");
            entity.Property(e => e.Ishide)
                .IsRequired()
                .HasPrecision(1)
                .HasDefaultValueSql("(0) ")
                .HasColumnName("ISHIDE");
            entity.Property(e => e.Isssprole)
                .IsRequired()
                .HasPrecision(1)
                .HasDefaultValueSql("(0) ")
                .HasColumnName("ISSSPROLE");
            entity.Property(e => e.Lastmodifiedby)
                .HasPrecision(10)
                .ValueGeneratedOnAdd()
                .HasDefaultValueSql("-1 ")
                .HasColumnName("LASTMODIFIEDBY");
            entity.Property(e => e.Lastmodifiedbytype)
                .HasPrecision(10)
                .ValueGeneratedOnAdd()
                .HasDefaultValueSql("-1 ")
                .HasColumnName("LASTMODIFIEDBYTYPE");
            entity.Property(e => e.Name)
                .HasMaxLength(256)
                .ValueGeneratedOnAdd()
                .HasColumnName("NAME");
            entity.Property(e => e.Profileid)
                .HasPrecision(10)
                .HasDefaultValueSql("(-1) ")
                .HasColumnName("PROFILEID");
            entity.Property(e => e.Rolecode)
                .HasMaxLength(50)
                .HasColumnName("ROLECODE");
            entity.Property(e => e.Script)
                .HasColumnType("CLOB")
                .HasColumnName("SCRIPT");
            entity.Property(e => e.Uniqueid)
                .HasMaxLength(36)
                .IsUnicode(false)
                .ValueGeneratedOnAdd()
                .IsFixedLength()
                .HasColumnName("UNIQUEID");
        });
        modelBuilder.HasSequence("ASSIGNMENTACTIONHISTORY_ID");
        modelBuilder.HasSequence("BADGEMEMBERS_ID");
        modelBuilder.HasSequence("BIGDATACONNECTIONS_INSTANCEID");
        modelBuilder.HasSequence("BNI_FAQ_SEQ_30082022");
        modelBuilder.HasSequence("CALLSCRIPTRESULTANSWERS_SEQ");
        modelBuilder.HasSequence("CANVASDESCRIPTION_SEQ");
        modelBuilder.HasSequence("CHATCUSTOMER_SETTINGID");
        modelBuilder.HasSequence("CHATMESSAGE_ID");
        modelBuilder.HasSequence("CHATWIDGET_SETTINGID");
        modelBuilder.HasSequence("CHATWIDGETSETTING_SETTINGID");
        modelBuilder.HasSequence("CIBILAPPLICATIONDATA_TOKENID");
        modelBuilder.HasSequence("CIS_LOG_SEQ");
        modelBuilder.HasSequence("CIS_RECORDLOG_SEQ");
        modelBuilder.HasSequence("CIS_RPTEXECONTEXT_CONTEXTID");
        modelBuilder.HasSequence("COMPANYSETTINGHISTORY_SEQ");
        modelBuilder.HasSequence("CONTACTSYNC_CONTACTSYNCID");
        modelBuilder.HasSequence("CONTENTFORMAT_SEQ");
        modelBuilder.HasSequence("CONTENTRECENTACTIVITY_SEQ");
        modelBuilder.HasSequence("CONVERSATIONOBJECTMAPPING_SEQ");
        modelBuilder.HasSequence("COURIERRELATED_MAPPINGID");
        modelBuilder.HasSequence("CRMMAPPEREXECUTIONSTATUS_SEQ");
        modelBuilder.HasSequence("DBOBJECTID_SEQUENCE").IncrementsBy(50);
        modelBuilder.HasSequence("DBTRACE_SEQ");
        modelBuilder.HasSequence("DNCCONFIGURATIONHISTORY_SEQ");
        modelBuilder.HasSequence("EMAILPOLLINGLOG_SEQ");
        modelBuilder.HasSequence("EMAILSERVERSETTINGS_SEQ");
        modelBuilder.HasSequence("EMAILSTATUS_ID");
        modelBuilder.HasSequence("FLOWHISTORY_HISTORYID_SEQ");
        modelBuilder.HasSequence("FLOWHISTORY_SEQ");
        modelBuilder.HasSequence("FLOWSTAGETRACE_EXECUTIONID");
        modelBuilder.HasSequence("FLOWSTATETRACE_TRACEID");
        modelBuilder.HasSequence("FORECAST_OLD_SEQ");
        modelBuilder.HasSequence("FRAGREPORT_SEQ");
        modelBuilder.HasSequence("GOALMEMBERS_ID");
        modelBuilder.HasSequence("GRAPHNOTE_NOTEID");
        modelBuilder.HasSequence("HDFC_CPU_REJECT_SEQ");
        modelBuilder.HasSequence("HDFC_DWH_PROD_DETAILS_TEMP_SEQ");
        modelBuilder.HasSequence("HDFC_DWH_PROD_SUMMARY_TEMP_SEQ");
        modelBuilder.HasSequence("IDGEN_SEQ_955_1030").IsCyclic();
        modelBuilder.HasSequence("IDGEN_SEQ_955_361").IsCyclic();
        modelBuilder.HasSequence("IDGEN_SEQ_955_61").IsCyclic();
        modelBuilder.HasSequence("IDGENHS_506_132_SEQ");
        modelBuilder.HasSequence("IDGENHS_506_19_SEQ");
        modelBuilder.HasSequence("IDGENHS_506_277_SEQ");
        modelBuilder.HasSequence("IDGENHS_506_5_SEQ");
        modelBuilder.HasSequence("IDGENHS_506_52_SEQ");
        modelBuilder.HasSequence("IDGENHS_506_56_SEQ");
        modelBuilder.HasSequence("IDGENHS_506_6_SEQ");
        modelBuilder.HasSequence("IDGENHS_506_7_SEQ");
        modelBuilder.HasSequence("IDGENHS_506_8_SEQ");
        modelBuilder.HasSequence("IDGENHS_506_9_SEQ");
        modelBuilder.HasSequence("IDGENHS_955_199_SEQ");
        modelBuilder.HasSequence("IDLIST_GEN_SEQ_HS_608_100034");
        modelBuilder.HasSequence("IDLIST_GEN_SEQ_HS_608_132");
        modelBuilder.HasSequence("IDLIST_GEN_SEQ_HS_608_19");
        modelBuilder.HasSequence("IDLIST_GEN_SEQ_HS_608_236");
        modelBuilder.HasSequence("IDLIST_GEN_SEQ_HS_608_277");
        modelBuilder.HasSequence("IDLIST_GEN_SEQ_HS_608_278");
        modelBuilder.HasSequence("IDLIST_GEN_SEQ_HS_608_359");
        modelBuilder.HasSequence("IDLIST_GEN_SEQ_HS_608_5");
        modelBuilder.HasSequence("IDLIST_GEN_SEQ_HS_608_52");
        modelBuilder.HasSequence("IDLIST_GEN_SEQ_HS_608_56");
        modelBuilder.HasSequence("IDLIST_GEN_SEQ_HS_608_6");
        modelBuilder.HasSequence("IDLIST_GEN_SEQ_HS_608_7");
        modelBuilder.HasSequence("IDLIST_GEN_SEQ_HS_608_8");
        modelBuilder.HasSequence("IDLIST_GEN_SEQ_HS_608_9");
        modelBuilder.HasSequence("IDLIST_GEN_SEQ_HS_955_199");
        modelBuilder.HasSequence("IDLIST_GEN_SEQ_HS_955_6");
        modelBuilder.HasSequence("IDLIST_GEN_SEQ_HS_955_9");
        modelBuilder.HasSequence("INTEGRATIONWEBAPI_SNO");
        modelBuilder.HasSequence("LAYOUTGROUPVIEW_SEQ");
        modelBuilder.HasSequence("LISTINGUTRMAPPINGHISTORY_SEQ");
        modelBuilder.HasSequence("LOCATIONTRACE_LOCATIONID");
        modelBuilder.HasSequence("LOGINHISTORY_SEQ");
        modelBuilder.HasSequence("MAPPEROUTPUTCONFIGURATION_SEQ");
        modelBuilder.HasSequence("MASHUPIDLIST_SEQ");
        modelBuilder.HasSequence("MASHUPPARAMMAPPING_MAPPINGID");
        modelBuilder.HasSequence("MASHUPQUERY_PARAMETERORDER");
        modelBuilder.HasSequence("MASSPRINTLOG_LOGID");
        modelBuilder.HasSequence("MDRS_3D926$");
        modelBuilder.HasSequence("MDRS_3D92D$");
        modelBuilder.HasSequence("MDRS_3D934$");
        modelBuilder.HasSequence("MDRS_3D93B$");
        modelBuilder.HasSequence("MEMBERACTIONS_TRACKERID");
        modelBuilder.HasSequence("MESSAGETYPESTATUS_ID");
        modelBuilder.HasSequence("MOBILEOBJECTS_INCREMENTID");
        modelBuilder.HasSequence("MOBILETHEME_THEMEID");
        modelBuilder.HasSequence("NEXTID_SEQ");
        modelBuilder.HasSequence("NOTIFICATIONS_INBOX_ID");
        modelBuilder.HasSequence("NOTIFICATIONS_NEW_ID");
        modelBuilder.HasSequence("NOTIFICATIONS_QUEUE_ID");
        modelBuilder.HasSequence("OBJECTMASTER_SEQ");
        modelBuilder.HasSequence("OBJECTQUERYMASTER_SEQ");
        modelBuilder.HasSequence("OBJECTRELATIONSHIP_ROWID");
        modelBuilder.HasSequence("OBJECTSHARINGACCESS_ID");
        modelBuilder.HasSequence("OBJECTSHARINGACCESS_SEQ");
        modelBuilder.HasSequence("ORIGINALPOST_POSTID");
        modelBuilder.HasSequence("OTPLOG_REFNO");
        modelBuilder.HasSequence("POLLEDEMAILS_SEQ");
        modelBuilder.HasSequence("POSTCOMMENT_COMMENTID");
        modelBuilder.HasSequence("PROFILECARD_ID");
        modelBuilder.HasSequence("PROJECTMEMBERS_MAPPINGID");
        modelBuilder.HasSequence("PURGESETTINGS_PURGEID");
        modelBuilder.HasSequence("PUSHNOTIFY_ID");
        modelBuilder.HasSequence("RE_OBJECTSHARINGHISTORY_SEQ");
        modelBuilder.HasSequence("SAMLSETTINGS_SAMLID");
        modelBuilder.HasSequence("SCHEMALOG_LOGID");
        modelBuilder.HasSequence("SCREENFLOWHISTORY_HISTORYID");
        modelBuilder.HasSequence("SCREENFLOWINSTANCE_INSTANCEID");
        modelBuilder.HasSequence("screenflowtrace_ID");
        modelBuilder.HasSequence("SEQ_ID_955_EDS_LOGID").IsCyclic();
        modelBuilder.HasSequence("SEQ_ID_955_NOTIF_LOGID").IsCyclic();
        modelBuilder.HasSequence("SEQ_ITEMS_TOBE_DELETED");
        modelBuilder.HasSequence("SMARTACTIVITYOFFLINE_ID");
        modelBuilder.HasSequence("SMSLOG_SMSID");
        modelBuilder.HasSequence("SOCIALNETWORKGROUPASSIGNMENT_S");
        modelBuilder.HasSequence("SQN_DAVIDINSTANCE_INSTANCEID");
        modelBuilder.HasSequence("SQN_DAVIDTRACE_INSTANCEID");
        modelBuilder.HasSequence("SQN_EMAILPROVIDER_PROVIDERID");
        modelBuilder.HasSequence("SQN_RULEHISTORY_HISTORYID");
        modelBuilder.HasSequence("STEPEXECUTION_STEPEXECUTIONID");
        modelBuilder.HasSequence("TABLESCHEMAEXTENDED_SEQ");
        modelBuilder.HasSequence("TEAMMEMBERTERRITORIES_SEQ");
        modelBuilder.HasSequence("TEMP_ADDDOC_SEQ");
        modelBuilder.HasSequence("TEMP_CONVERSATIONMAPPING_SEQ");
        modelBuilder.HasSequence("TEMP_ORG_HIERARCHY_SEQ");
        modelBuilder.HasSequence("TEMP_ORGANIZATIONAL_HIERACHY");
        modelBuilder.HasSequence("TEMPOBJECTINSTANCE_TEMPID");
        modelBuilder.HasSequence("TESTCASEEXECUTION_EXECUTIONID");
        modelBuilder.HasSequence("TESTCASEVERSION_VERSIONID");
        modelBuilder.HasSequence("TMP_SFTRACE_TRACEID");
        modelBuilder.HasSequence("TMP_SFTRACEHISTORY_UNIID");
        modelBuilder.HasSequence("TT_FLOWDRAFTVERSIONS");
        modelBuilder.HasSequence("TT_KANBANSETTING");
        modelBuilder.HasSequence("TT_LOGACALLSETTING");
        modelBuilder.HasSequence("TT_MERGEOBJECTLOGGING_ID");
        modelBuilder.HasSequence("TT_MOBILEAPKCOMPATIBILITY_A");
        modelBuilder.HasSequence("TT_QCEXECUTIONGRIDDATA_RECO");
        modelBuilder.HasSequence("TT_TRINITYOBJECTCARDDATA_RE");
        modelBuilder.HasSequence("TT_TRINITYPRODUCTLINKS_LINK");
        modelBuilder.HasSequence("TT_TRINITYPRODUCTMAPPING_MA");
        modelBuilder.HasSequence("TT_TRUECALLER_A");
        modelBuilder.HasSequence("TT_VIVIDCATEGORY_ID");
        modelBuilder.HasSequence("TT_VIVIDSUBCATEGORY_SUBID");
        modelBuilder.HasSequence("USERAVAILABILITY_ID");
        modelBuilder.HasSequence("USERSCOPEVISIBILITY_SEQ");
        modelBuilder.HasSequence("USERTERRITORY_SEQ");
        modelBuilder.HasSequence("WORKRESULT_RESULTID");

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
