/* Yet Another Forum.NET
 * Copyright (C) 2003-2005 Bjørnar Henden
 * Copyright (C) 2006-2013 Jaben Cargman
 * Copyright (C) 2014-2015 Ingo Herbote
 * http://www.yetanotherforum.net/
 * 
 * Licensed to the Apache Software Foundation (ASF) under one
 * or more contributor license agreements.  See the NOTICE file
 * distributed with this work for additional information
 * regarding copyright ownership.  The ASF licenses this file
 * to you under the Apache License, Version 2.0 (the
 * "License"); you may not use this file except in compliance
 * with the License.  You may obtain a copy of the License at

 * http://www.apache.org/licenses/LICENSE-2.0

 * Unless required by applicable law or agreed to in writing,
 * software distributed under the License is distributed on an
 * "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY
 * KIND, either express or implied.  See the License for the
 * specific language governing permissions and limitations
 * under the License.
 */

namespace YAF.Pages
{
    // YAF.Pages
    #region Using

    using System;
    using System.Data;
    using System.Web;

    using YAF.Classes.Data;
    using YAF.Controls;
    using YAF.Core;
    using YAF.Core.Services;
    using YAF.Types;
    using YAF.Types.Constants;
    using YAF.Types.Interfaces;
    using YAF.Types.Models;
    using YAF.Utils;
    using YAF.Core.Extensions;

    #endregion

    /// <summary>
    /// Summary description for emailtopic.
    /// </summary>
    public partial class emailtopic : ForumPage
    {
        #region Constructors and Destructors

        /// <summary>
        ///   Initializes a new instance of the <see cref = "emailtopic" /> class.
        /// </summary>
        public emailtopic()
            : base("EMAILTOPIC")
        {
        }

        #endregion

        #region Methods

        /// <summary>
        /// The page_ load.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        protected void Page_Load([NotNull] object sender, [NotNull] EventArgs e)
        {
            if (this.Get<HttpRequestBase>().QueryString["t"] == null || !this.PageContext.ForumReadAccess ||
                !this.PageContext.BoardSettings.AllowEmailTopic)
            {
                YafBuildLink.AccessDenied();
            }

            if (!this.IsPostBack)
            {
                if (this.PageContext.Settings.LockedForum == 0)
                {
                    this.PageLinks.AddLink(this.PageContext.BoardSettings.Name, YafBuildLink.GetLink(ForumPages.forum));
                    this.PageLinks.AddLink(
                      this.PageContext.PageCategoryName,
                      YafBuildLink.GetLink(ForumPages.forum, "c={0}", this.PageContext.PageCategoryID));
                }

                this.PageLinks.AddForum(this.PageContext.PageForumID);
                this.PageLinks.AddLink(
                  this.PageContext.PageTopicName, YafBuildLink.GetLink(ForumPages.posts, "t={0}", this.PageContext.PageTopicID));

                this.SendEmail.Text = this.GetText("send");

                this.Subject.Text = this.PageContext.PageTopicName;

                var emailTopic = new YafTemplateEmail();

                emailTopic.TemplateParams["{link}"] = YafBuildLink.GetLinkNotEscaped(
                  ForumPages.posts, true, "t={0}", this.PageContext.PageTopicID);
                emailTopic.TemplateParams["{user}"] = this.PageContext.PageUserName;

                this.Message.Text = emailTopic.ProcessTemplate("EMAILTOPIC");
            }
        }

        /// <summary>
        /// The send email_ click.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        protected void SendEmail_Click([NotNull] object sender, [NotNull] EventArgs e)
        {
            if (this.EmailAddress.Text.Length == 0)
            {
                this.PageContext.AddLoadMessage(this.GetText("need_email"));
                return;
            }

            try
            {
                string senderEmail = null;

                using (DataTable dt = LegacyDb.user_list(this.PageContext.PageBoardID, this.PageContext.PageUserID, true))
                {
                    senderEmail = (string)dt.Rows[0]["Email"];
                }

                // send the email...
                this.Get<ISendMail>().Send(
                  senderEmail, this.EmailAddress.Text.Trim(), this.Subject.Text.Trim(), this.Message.Text.Trim());

                YafBuildLink.Redirect(ForumPages.posts, "t={0}", this.PageContext.PageTopicID);
            }
            catch (Exception x)
            {
                this.Logger.Log(this.PageContext.PageUserID, this, x);
                this.PageContext.AddLoadMessage(this.GetTextFormatted("failed", x.Message));
            }
        }

        #endregion
    }
}