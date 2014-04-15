=============================================================================
          APPLICATION : CSUACSelfElevation Project Overview
=============================================================================

/////////////////////////////////////////////////////////////////////////////
Summary: 

User Account Control (UAC) is a new security component in Windows Vista and 
newer operating systems. With UAC fully enabled, interactive administrators 
normally run with least user privileges. This example demonstrates how to
self-elevate the process by giving explicit consent with the Consent UI. 


/////////////////////////////////////////////////////////////////////////////
Prerequisite:

You must run this sample on Windows Vista or newer operating systems.


/////////////////////////////////////////////////////////////////////////////
Demo:

The following steps walk through a demonstration of the UAC sample.

Step1. After you successfully build the sample project in Visual Studio 2008, 
you will get an application: CSUACSelfElevation.exe. 

Step2. Run the application as a protected administrator on a Windows Vista or 
Windows 7 system with UAC fully enabled. Click on the Self-elevate button. 
You will see a Consent UI.

  User Account Control
  ---------------------------------- 
  Do you want to allow the following program from an unknown publisher to 
  make changes to this computer?

Step3. Click Yes to approve the elevation. The original application will then 
be restarted, running as an elevated administrator.

Step4. Close the application. 


/////////////////////////////////////////////////////////////////////////////
Implementation:

Step1. Create a new Visual C# Windows Forms project named CSUACSelfElevation.

Step2. Add the following button to the main form.

  Type: Button
  ID: btnElevate
  Caption: "Self-elevate"

In MainForm_Load, update the button to show a UAC shield icon. The icon 
indicates that the task requires elevation.

    // Update the Self-elevate button to show a UAC shield icon.
    this.btnElevate.FlatStyle = FlatStyle.System;
    SendMessage(btnElevate.Handle, BCM_SETSHIELD, 0, (IntPtr)1);

Handle its click event of the button so that when user clicks it, it will 
elevate the process by restarting itself with 
ProcessStartInfo.UseShellExecute = true and ProcessStartInfo.Verb = "runas".

    private void btnElevate_Click(object sender, EventArgs e)
    {
        // Launch itself as administrator
        ProcessStartInfo proc = new ProcessStartInfo();
        proc.UseShellExecute = true;
        proc.WorkingDirectory = Environment.CurrentDirectory;
        proc.FileName = Application.ExecutablePath;
        proc.Verb = "runas";

        try
        {
            Process.Start(proc);
        }
        catch
        {
            // The user refused to allow privileges elevation.
            // Do nothing and return directly ...
            return;
        }

        Application.Exit();  // Quit itself
    }

Step3. Automatically elevate the process when it's started up.

If your application always requires administrative privileges, such as during 
an installation step, the operating system can automatically prompt the user 
for privileges elevation each time your application is invoked. 

If a specific kind of resource (RT_MANIFEST) is found embedded within the 
application executable, the system looks for the <trustInfo> section and 
parses its contents. Here is an example of this section in the manifest file:

    <trustInfo xmlns="urn:schemas-microsoft-com:asm.v2">
       <security>
          <requestedPrivileges>
             <requestedExecutionLevel
                level="requireAdministrator"
             />
          </requestedPrivileges>
       </security>
    </trustInfo>

Three different values are possible for the level attribute

  a) requireAdministrator 
  The application must be started with Administrator privileges; it won't run 
  otherwise.

  b) highestAvailable 
  The application is started with the highest possible privileges.
  If the user is logged on with an Administrator account, an elevation prompt 
  appears. If the user is a Standard User, the application is started 
  (without any elevation prompt) with these standard privileges.

  c) asInvoker 
  The application is started with the same privileges as the calling 
  application.

To configure the elevation level in this Visual C# Windows Forms project, 
open the project's properties, turn to the Security tab, check the checkbox 
"Enable ClickOnce Security Settings", check "This is a fulltrust application" 
and close the application Properies page. This creates an app.manifest file 
and configures the project to embed the manifest. You can open the 
"app.manifest" file from Solution Explorer by expanding the Properies folder. 
The file has the following content by default.

    <?xml version="1.0" encoding="utf-8"?>
    <asmv1:assembly manifestVersion="1.0" xmlns="urn:schemas-microsoft-com:asm.v1" 
    xmlns:asmv1="urn:schemas-microsoft-com:asm.v1" xmlns:asmv2="urn:schemas-microsoft-com:asm.v2" 
    xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
      <assemblyIdentity version="1.0.0.0" name="MyApplication.app" />
      <trustInfo xmlns="urn:schemas-microsoft-com:asm.v2">
        <security>
          <requestedPrivileges xmlns="urn:schemas-microsoft-com:asm.v3">
            <!-- UAC Manifest Options
                If you want to change the Windows User Account Control level replace the 
                requestedExecutionLevel node with one of the following.

            <requestedExecutionLevel  level="asInvoker" uiAccess="false" />
            <requestedExecutionLevel  level="requireAdministrator" uiAccess="false" />
            <requestedExecutionLevel  level="highestAvailable" uiAccess="false" />

                If you want to utilize File and Registry Virtualization for backward 
                compatibility then delete the requestedExecutionLevel node.
            -->
            <requestedExecutionLevel level="asInvoker" uiAccess="false" />
          </requestedPrivileges>
          <applicationRequestMinimum>
            <PermissionSet class="System.Security.PermissionSet" version="1" 
            Unrestricted="true" ID="Custom" SameSite="site" />
            <defaultAssemblyRequest permissionSetReference="Custom" />
          </applicationRequestMinimum>
        </security>
      </trustInfo>
    </asmv1:assembly>

Here we are focusing on the line:

    <requestedExecutionLevel level="asInvoker" uiAccess="false" />

You can change it to be 

    <requestedExecutionLevel level="requireAdministrator" uiAccess="false" />

to require the application always be started with Administrator privileges.


/////////////////////////////////////////////////////////////////////////////
References:

MSDN: User Account Control
http://msdn.microsoft.com/en-us/library/aa511445.aspx

MSDN: Windows Vista Application Development Requirements for User Account 
Control Compatibility
http://msdn.microsoft.com/en-us/library/bb530410.aspx


/////////////////////////////////////////////////////////////////////////////