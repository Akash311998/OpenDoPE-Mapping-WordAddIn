﻿//Copyright (c) Microsoft Corporation.  All rights reserved.
/*
 *  From http://xmlmapping.codeplex.com/license:

    Microsoft Platform and Application License

    This license governs use of the accompanying software. If you use the software, you accept this license. If you 
    do not accept the license, do not use the software.

    1. Definitions
    The terms “reproduce,” “reproduction,” “derivative works,” and “distribution” have the same meaning here as 
    under U.S. copyright law.
    A “contribution” is the original software, or any additions or changes to the software.
    A “contributor” is any person that distributes its contribution under this license.
    “Licensed patents” are a contributor’s patent claims that read directly on its contribution.

    2. Grant of Rights
    (A) Copyright Grant- Subject to the terms of this license, including the license conditions and limitations in 
    section 3, each contributor grants you a non-exclusive, worldwide, royalty-free copyright license to reproduce 
    its contribution, prepare derivative works of its contribution, and distribute its contribution or any derivative 
    works that you create.
    (B) Patent Grant- Subject to the terms of this license, including the license conditions and limitations in section 
    3, each contributor grants you a non-exclusive, worldwide, royalty-free license under its licensed patents to 
    make, have made, use, sell, offer for sale, import, and/or otherwise dispose of its contribution in the software 
    or derivative works of the contribution in the software.

    3. Conditions and Limitations
    (A) No Trademark License- This license does not grant you rights to use any contributors’ name, logo, or
    trademarks.
    (B) If you bring a patent claim against any contributor over patents that you claim are infringed by the
    software, your patent license from such contributor to the software ends automatically.
    (C) If you distribute any portion of the software, you must retain all copyright, patent, trademark, and
    attribution notices that are present in the software.
    (D) If you distribute any portion of the software in source code form, you may do so only under this license
    by including a complete copy of this license with your distribution. If you distribute any portion of the 
    software in compiled or object code form, you may only do so under a license that complies with this license.
    (E) The software is licensed “as-is.” You bear the risk of using it. The contributors give no express warranties, 
    guarantees or conditions. You may have additional consumer rights under your local laws which this license 
    cannot change. To the extent permitted under your local laws, the contributors exclude the implied warranties 
    of merchantability, fitness for a particular purpose and non-infringement.
    (F) Platform Limitation- The licenses granted in sections 2(A) & 2(B) extend only to the software or derivative
    works that you create that (1) run on a Microsoft Windows operating system product, and (2) operate with 
    Microsoft Word.
 */
using System;
using System.Diagnostics;
using System.Windows.Forms;
using Microsoft.Win32;
using XmlMappingTaskPane.Controls;

namespace XmlMappingTaskPane.Forms
{
    public partial class FormOptions : Form
    {
        private int m_grfOptions;

        public FormOptions()
        {
            InitializeComponent();
        }

        internal int NewOptions
        {
            get
            {
                return m_grfOptions;
            }
        }

        private void FormOptions_Load(object sender, EventArgs e)
        {
            //set up the options
            try
            {
                m_grfOptions = (int)Registry.CurrentUser.OpenSubKey(System.Configuration.ConfigurationManager.AppSettings["Registry.CurrentUser.SubKey"]).GetValue("Options");
            }
            catch (NullReferenceException nrex)
            {
                Debug.Assert(false, "Regkey corruption", "either the user manually deleted the regkeys, or something bad happened." + Environment.NewLine + nrex.Message);
            }

            //options exist

            //set attributes
            if ((m_grfOptions & ControlTreeView.cOptionsShowAttributes) != 0)
                checkBoxAttributes.Checked = true;
            else
                checkBoxAttributes.Checked = false;

            //set comments
            if ((m_grfOptions & ControlTreeView.cOptionsShowComments) != 0)
                checkBoxComments.Checked = true;
            else
                checkBoxComments.Checked = false;

            //set PIs
            if ((m_grfOptions & ControlTreeView.cOptionsShowPI) != 0)
                checkBoxPI.Checked = true;
            else
                checkBoxPI.Checked = false;

            //set text
            if ((m_grfOptions & ControlTreeView.cOptionsShowText) != 0)
                checkBoxText.Checked = true;
            else
                checkBoxText.Checked = false;

            //set property page
            if ((m_grfOptions & ControlTreeView.cOptionsShowPropertyPage) != 0)
                checkBoxProperties.Checked = true;
            else
                checkBoxProperties.Checked = false;

            //set autoselect
            if ((m_grfOptions & ControlTreeView.cOptionsAutoSelectNode) != 0)
                checkBoxAutomaticallySelect.Checked = true;
            else
                checkBoxAutomaticallySelect.Checked = false;
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            //set options
            int grfOptions = 0;

            //set up the bitflag
            if (checkBoxAttributes.Checked == true)
                grfOptions = grfOptions | ControlTreeView.cOptionsShowAttributes;

            if (checkBoxComments.Checked == true)
                grfOptions = grfOptions | ControlTreeView.cOptionsShowComments;

            if (checkBoxPI.Checked == true)
                grfOptions = grfOptions | ControlTreeView.cOptionsShowPI;

            if (checkBoxText.Checked == true)
                grfOptions = grfOptions | ControlTreeView.cOptionsShowText;

            if (checkBoxProperties.Checked == true)
                grfOptions = grfOptions | ControlTreeView.cOptionsShowPropertyPage;

            if (checkBoxAutomaticallySelect.Checked == true)
                grfOptions = grfOptions | ControlTreeView.cOptionsAutoSelectNode;

            //persist the options
            if (grfOptions != m_grfOptions)
            {
                m_grfOptions = grfOptions;

                try
                {
                    if (Registry.CurrentUser.OpenSubKey(System.Configuration.ConfigurationManager.AppSettings["Registry.CurrentUser.SubKey"]) == null)
                        Registry.CurrentUser.CreateSubKey(System.Configuration.ConfigurationManager.AppSettings["Registry.CurrentUser.SubKey"], RegistryKeyPermissionCheck.ReadWriteSubTree);

                    using (RegistryKey rk = Registry.CurrentUser.OpenSubKey(System.Configuration.ConfigurationManager.AppSettings["Registry.CurrentUser.SubKey"], true))
                        rk.SetValue("Options", m_grfOptions);
                }
                catch (System.Security.SecurityException)
                {                    
                    ControlBase.ShowErrorMessage(this, Properties.Resources.ErrorSaveSettings);
                }

                //set the result to OK
                DialogResult = DialogResult.OK;
            }

            //close the form
            this.Close();
        }
    }
}