/****** Object:  Table [dbo].[MobileLangDetail]    Script Date: 9/5/2018 4:13:20 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MobileLangDetail](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[LangId] [int] NOT NULL,
	[Name] [nvarchar](50) NULL,
	[Value] [ntext] NULL,
 CONSTRAINT [PK_MobileLangDetail] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

/****** Object:  Table [dbo].[MobileLangMsgDetail]    Script Date: 9/5/2018 4:13:20 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MobileLangMsgDetail](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[LangId] [int] NOT NULL,
	[Name] [nvarchar](50) NULL,
	[Value] [ntext] NULL,
 CONSTRAINT [PK_MobileLangMsgDetail] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

USE [CPL]
GO
SET IDENTITY_INSERT [dbo].[MobileLangMsgDetail] ON 
GO
INSERT [dbo].[MobileLangMsgDetail] ([Id], [LangId], [Name], [Value]) VALUES (1, 1, N'PrivacyPolicy', N'Website of www.cryptolot.com (hereinafter "the service") is operated by A Company (hereinafter "our company").
                                In this page we will inform you of the policies concerning the collection, use and disclosure of personal information when using this service and the policy of data handling by customers using this service (hereinafter "user").
                                We will use your data to provide and improve our services. By using this service, you agree to collect and use information in accordance with this policy. Unless otherwise defined in this Privacy Policy, terms used in this Privacy Policy have the same meaning as our terms of service accessible from www.cryptolot.com.
                                <ol class="text-justify pl-1">
                                    <li>
                                        <h5 class="font-weight-bold">Scope of this policy</h5>
                            <p>We handle customer''s personal information for achieving its purpose of use in various services provided by our company. We will not use your personal information beyond the purpose of use indicated to you unless it falls under the exception specified by the Personal Information Protection Act.</p>
                            </li>
                            <li>
                                <h5 class="font-weight-bold">Types of personal data to be collected</h5>
                                <p>
                                    While using this service you may ask us to provide specific personal identification information ("Personal Data") that we can use to contact you or verify your identity. Personally identifiable information includes (but is not limited to): e-mail address, last name and first name, phone number, address (state, province, zip code, city), driver''s license number, passport number, cookies and usage data.
                                </p>
                                <ol class="pl-1">
                                    <li>
                                        <h6>Collection of use data of this service</h5>
                                        <p>This service may collect information on how the service is accessed and used ("usage data"). This usage data includes the internet protocol address (IP address, etc.) of your computer, the type of browser, the browser version, the page of the accessed service, the date and time of visit, the time spent on those pages, the device identifier and other Contains diagnostic data.</p>
                                    </li>
                                    <li>
                                        <h6>Tracking & cookie data</h5>
                                        <p>
                                            We use cookies and similar tracking techniques to keep track of user activity and maintain certain information.
                                            A cookie is a file that contains a small amount of data that may contain anonymous unique identifiers. Cookies are sent from the website to the browser and stored on the device. The user can tell the browser to deny all cookies and to indicate that the cookie has been sent. However, if you do not accept cookies, you may not be able to use some of our services.
                                        </p>
                                    </li>
                                </ol>
                            </li>
                            <li>
                                <h5 class="font-weight-bold">About the use of collected personal data</h5>
                                <p>With this service, we will use personal information acquired within the scope of the following purpose of use.</p>
                                <ol class="pl-1">
                                    <li>Maintaining the quality of services provided by us</li>
                                    <li>Notice about change of service provided by our company</li>
                                    <li>Permission to participate in interactive functions of services provided by us</li>
                                    <li>Providing support services to users</li>
                                    <li>Collect opinions for improving services provided by the Company</li>
                                    <li>Monitor usage of services provided by the Company</li>
                                    <li>Detect and cope with technical problems, create preventive measures</li>
                                </ol>
                            </li>
                            <li>
                                <h5 class="font-weight-bold mt-1">Data transfer</h5>
                                <p>
                                    Your information, including personal data, may be transferred and maintained to computers outside the state, country or other jurisdiction where the data protection law may differ from your jurisdictional area.
                                    Please be aware that if you choose to migrate outside and choose to provide information to us, you may transfer data including personal data to other countries and process it.
                                    The consent to this privacy policy and the submission of personal information represent the user''s consent to the transfer.
                                    This service will take all necessary measures to ensure that your data is safely handled in accordance with this Privacy Policy.
                                </p>
                            </li>
                            <li>
                                <h5 class="font-weight-bold">Disclosure of data</h5>
                                <p>This service may disclose your personal information for the following purposes.</p>
                                <ul class="pl-1">
                                    <li>To comply with legal obligations and to protect legal liability</li>
                                    <li>To prevent or investigate cheating related to this service</li>
                                    <li>To protect personal safety of this service or user </li>
                                </ul>
                            </li>
                            <li>
                                <h5 class="font-weight-bold mt-1">Data security</h5>
                                <p>The security of your data is important to us, but the sending method and electronic storage method on the Internet are not 100% secure. We strive to protect your personal data by using commercially acceptable measure, but do not guarantee absolute safety.</p>
                            </li>
                            <li>
                                <h5 class="font-weight-bold">Service Provider</h5>
                                <p>
                                    This service is intended to enable third party companies and individuals to collect data for the purpose of facilitating the service, providing services acting on behalf of the service, implementing related services, or analyzing how to use the service It may be used.
                                    These third parties are obliged to access your personal data only to do these work for us and not disclose or use it for other purposes.
                                </p>
                            </li>
                            <li>
                                <h5 class="font-weight-bold">Link to another site</h5>
                                <p>We do not install any of our products which require this (osCommerce templates, Zen Cart templates, etc.). Installation is a paid service which is not included in the package price.</p>
                            </li>
                            <li>
                                <h5 class="font-weight-bold">Children''s privacy</h5>
                                <p>
                                    Our service does not deal with those under the age of 18 ("children").
                                    We will deliberately never collect personal information from those under the age of 18. If you know that you are a parent or guardian and your child is providing us with personal information, please contact us. If you notice that collecting personal data from your child without confirming your parent''s consent, follow the procedure to remove that information from the server.
                                </p>
                            </li>
                            <li>
                                <h5 class="font-weight-bold">Changes to this privacy policy</h5>
                                <p>From time to time, we may update our privacy policy. By posting a new privacy policy on this page we will inform you of changes.</p>
                            </li>
                            </ol>company").')
GO
INSERT [dbo].[MobileLangMsgDetail] ([Id], [LangId], [Name], [Value]) VALUES (2, 1, N'TermsOfService', N'This Terms of Service (the "Terms of Service") is provided by CRYPTOLOT (hereinafter referred to as "the Service") on the CRYPTOLOT website (hereinafter referred to as "the Website") It defines the usage conditions of the service.
                                <ol class="text-justify pl-1">
                                    <li>
                                        <h5 class="font-weight-bold">Application</h5>
                            <p>This terms apply to all relationships of users involved in using this service.</p>
                            </li>
                            <li>
                                <h5 class="font-weight-bold">Regarding usage registration</h5>
                                <ol class="pl-1">
                                    <li>Registration applicant applies for registration for use according to the method defined by this service, and this service approves this and registration of use will be completed.</li>
                                    <li>This service may not approve the application for use registration when judging that the applicant for use registration has the following reasons and does not undertake any disclosure obligation for the reason.</li>
                                    <ol class="pl-2">
                                        <li>the user submit a false information upon applying for registration of use</li>
                                        <li>If it is an application from a person who has violated these terms</li>
                                        <li>Other cases when this service judging that the user is not suit to use registration</li>
                                    </ol>
                                </ol>
                            </li>
                            <li>
                                <h5 class="font-weight-bold mt-1">User ID and password management</h5>
                                <ol class="pl-1">
                                    <li>The user manage the user ID and password of this service at its own risk.</li>
                                    <li>In no case can the user transfer or rent a user ID and password to a third party. If this user is logged in as a combination of user ID and password in accordance with the registration information, this service is considered to be used by the user who registered the user ID.</li>
                                    <li>Each player is allowed only one account. Users with many accounts, players using scripts and the like for the purpose of interfering with the service''s system are automatically blocked. Blocked players can not withdraw the crypto currency.</li>
                                </ol>
                            </li>
                            <li>
                                <h5 class="font-weight-bold mt-1">Managing tokens</h5>
                                <ol class="pl-1">
                                    <li>Tokens used in the lottery purchase of this service and used for exchanging crypto currency in this service are called CPL (hereinafter referred to as "token"). The tokens described in this agreement will indicate this CPL.</li>
                                    <li>This service reserves the right to close accounts that have not been active for more than 6 months and to retain related tokens. Your token is stored safely, but we do not recommend long-term storage. It is also recommended that you download the desktop wallet to store the crypto currency used to exchange tokens.</li>
                                    <li>The user exchanges the crypto currency and tokens according to the usage rate displayed separately on this website separately as a consideration for using this service. In addition, exchange of the crypto currency and the token shall be carried out by the method specified by this service.</li>
                                </ol>
                            </li>
                            <li>
                                <h5 class="font-weight-bold mt-1">Prohibitions</h5>
                                    <p>Users should not perform the following acts in using this service.</p>
                                    <ol class="pl-1">
                                        <li>Acts that violate laws or public order and morals</li>
                                        <li>Acts related to criminal acts</li>
                                        <li>Actions that destroy or interfere with the function of the server or network of this service</li>
                                        <li>Acts that may interfere with the operation of this service</li>
                                        <li>Acts of collecting or accumulating personal information etc. concerning other users</li>
                                        <li>Impersonating other users</li>
                                        <li>Acts of directly or indirectly giving profits to antisocial forces in connection with this service</li>
                                        <li>Other acts that the Service deems inappropriate</li>
                                    </ol>
                            </li>
                            <li>
                                <h5 class="font-weight-bold mt-1">Stopping the provide of this service, etc.</h5>
                                <ol class="pl-1">
                                    <li>This Service may stop of all or part of the Service without notifying the User in advance if it determines that there is any of the following reasons.</li>
                                    <ol class="pl-2">
                                        <li>When performing maintenance, inspection, or updating of the computer system related to this service</li>
                                        <li>When it becomes difficult to provide this service due to accidental force act such as earthquake, lightning strike, fire, blackout or natural disaster</li>
                                        <li>When a computer or a communication line stops due to an accident</li>
                                        <li>In addition, when judging that it is difficult to provide this service</li>
                                    </ol>
                                    <li>This service shall not be liable for any reason whatsoever for any disadvantage or damage suffered by the user or a third party due to suspension or interruption of the provision of this service.</li>
                                </ol>
                            </li>
                            <li>
                                <h5 class="font-weight-bold mt-1">Restrict usage and deregister</h5>
                                <ol class="pl-1">
                                    <li>This service reserves the right to change the system specification requirements necessary for changing the website, service, software without prior notice and accessing and using the service.</li>
                                    <li>This service may restrict the use of all or part of this service to the user, or may cancel the registration as a user, in the following cases without prior notice.</li>
                                    <ol class="pl-2">
                                        <li>In the event of violating any provision of these Terms</li>
                                        <li>When it is found that there is a false fact in the registration matter</li>
                                        <li>In addition, if we determine that this service is not appropriate for using this service</li>
                                    </ol>
                                    <li>This service is not responsible for any damage to the user caused by the actions performed by this service under this section.</li>
                                    <li>Use or access of this service by users under the age of 18 is prohibited. If you are under 18 years of age please do not use or provide information on this service.</li>
                                    <li>This Service exclusively reserves the right to refuse, cancel service, and / or refuse to distribute profits for good reason.</li>
                                </ol>
                            </li>
                            <li>
                                <h5 class="font-weight-bold mt-1">Deposit with crypto currency</h5>
                                <p>
                                    This service does not accept cash or payment. Only Bitcoin and Ethereum is accepted.
                                </p>
                            </li>
                            <li>
                                <h5 class="font-weight-bold">Withdrawal by crypto currency</h5>
                                <ol class="pl-1">
                                    <li>This service will withdraw Bitcoin and Ethereum. cash withdrawal is not possible. Also, when withdrawing crypto currency, you will need to prove your identity.</li>
                                    <li>Users are responsible for the validity of the information provided during identification.</li>
                                    <li>This service reserves the right to restrict or refuse withdrawal if false information is included in the user identification information.</li>
                                </ol>
                            </li>
                            <li>
                                <h5 class="font-weight-bold mt-1">Disclaimer</h5>
                                <ol class="pl-1">
                                    <li>This service will not refund anything due to malfunctioning of the game, server failure or malfunction, or loss caused as a result of the lottery purchase by the user. Moreover, we are not responsible for compensation.</li>
                                    <li>If the game malfunctions, all game play during the malfunction period will be invalid.</li>
                                    <li>When a user conducts cheating, using multiple accounts or misdemeaning, your account will be closed and your cryptocurrency and tokens, including investments, will be confiscated. Continued withdrawal of revenue is prohibited and user''s account is blocked.</li>
                                    <li>This service is not responsible for defects caused by increased traffic on the telephone network, line, computer online system, server or provider, hardware, software, technical problems or the Internet, website, service.</li>
                                    <li>This service is not responsible for data mistake, data storage and processing, damage caused by incompleteness and inaccuracy of transmitted data.</li>
                                    <li>This service is not responsible for transactions, contacts, disputes, etc. arising between users related to this service and other users or third parties.</li>
                                </ol>
                            </li>
                            <li>
                                <h5 class="font-weight-bold mt-1">Change of service contents</h5>
                                <p>
                                    This service shall be able to change the contents of this service or cancel the provision of this service without notifying the user and will not bear any responsibility for damage caused to the user by this.
                                </p>
                            </li>
                            <li>
                                <h5 class="font-weight-bold">Change of terms of service</h5>
                                <p>
                                    If we decide that this service is necessary, we may change this Terms at any time without notifying the user.
                                </p>
                            </li>
                            <li>
                                <h5 class="font-weight-bold">Notification or contact</h5>
                                <p>
                                    Notification or communication between the user and this service will be made according to the method specified by this service.
                                </p>
                            </li>
                            <li>
                                <h5 class="font-weight-bold">Prohibition of assignment of rights and duties</h5>
                                <p>
                                    You may not transfer the status under the contract or the rights or obligations under this Agreement to a third party without prior consent of the Service in writing and can not be used as collateral.
                                </p>
                            </li>
                            <li>
                                <h5 class="font-weight-bold">Governing Law · Jurisdiction</h5>
                                <p>
                                    In the event of a dispute with respect to this service, the court having jurisdiction over the location of the head office of this service shall be subject to exclusive agreement jurisdiction.
                                </p>
                            </li>
                            </ol>')
GO
INSERT [dbo].[MobileLangMsgDetail] ([Id], [LangId], [Name], [Value]) VALUES (3, 2, N'PrivacyPolicy', N'運営会社A（以下「当社」）はwww.cryptolot.comのウェブサイト（以下「本サービス」）を運営しています。
                                このページでは、本サービスを利用する際の個人情報の収集、使用、開示に関する方針、および本サービスを利用されるお客様(以下「ユーザー」)のデータの取り扱いの方針についてお知らせします。
                                当社はあなたのデータを使用してサービスを提供し改善します。本サービスを使用することにより、ユーザーは本ポリシーに従って情報の収集および使用に同意するものとします。このプライバシーポリシーで別途定義されていない限り、このプライバシーポリシーで使用される用語は、www.cryptolot.comからアクセス可能な当社の利用規約と同じ意味を持ちます。
                                <ol class="text-jutify pl-1">
                                    <li>
                                        <h5 class="font-weight-bold">本ポリシー適用範囲</h5>
                            <p>当社は、当社が提供する各種サービスにおいて、お客様の個人情報を、その利用目的の達成に必要な範囲において取り扱います。当社は、個人情報保護法で定める例外に該当する場合を除き、お客様に示した利用目的を超えてお客様の個人情報を利用いたしません。</p>
                            </li>
                            <li>
                                <h5 class="font-weight-bold">収集される個人データの種類</h5>
                                <p>
                                    本サービスを利用している間は、あなたに連絡したり身元を確認するために使用できる特定の個人識別情報（以下「個人データ」）を私たちに提供するよう求める場合があります。個人を特定できる情報には、以下が含まれます（これらに限定されない場合があります: 電子メールアドレス, 姓と名, 電話番号, 住所、州、県、郵便番号、市区町村, 運転免許証番号, パスポート番号, Cookieおよび使用状況データ
                                </p>
                                <ol pl-1>
                                    <li>
                                        <h5>本サービスの使用データの収集</h5>
                                        <p>本サービスは、本サービスがどのようにアクセスされ、使用されているかの情報（以下「使用データ」）も収集することがあります。この使用データには、お使いのコンピュータのインターネットプロトコルアドレス（IPアドレスなど）、ブラウザの種類、ブラウザのバージョン、アクセスしたサービスのページ、訪問日時、それらのページに費やした時間、デバイス識別子および他の診断データを含みます。 </p>
                                    </li>
                                    <li>
                                        <h5>トラッキング＆クッキーデータ</h5>
                                        <p>
                                            当社は、ユーザーの活動を追跡し、一定の情報を保持するためにCookieおよび類似の追跡技術を使用しています。
                                            Cookieは、匿名の一意の識別子を含む可能性のある少量のデータを含むファイルです。クッキーはWebサイトからブラウザに送信され、デバイスに保存されます。
                                            ユーザーはブラウザにすべてのCookieを拒否したり、Cookieが送信されたことを示すように指示することができます。ただし、Cookieを受け入れない場合は、当社のサービスの一部を使用することができない場合があります。
                                        </p>
                                    </li>
                                </ol>
                            </li>
                            <li>
                                <h5 class="font-weight-bold">収集した個人データの使用について </h5>
                                <p>本サービスでは、以下の利用目的の範囲内で取得した個人情報を利用いたします。</p>
                                <ul pl-1>
                                    <li>当社が提供するサービスの品質の維持</li>
                                    <li>当社が提供するサービスの変更について通知 </li>
                                    <li>当社が提供するサービスの対話型機能への参加への許可 </li>
                                    <li>ユーザーに対するサポートサービスの提供</li>
                                    <li>当社が提供するサービス改善のための意見の収集</li>
                                    <li>当社が提供するサービスの使用状況の監視</li>
                                    <li>技術的な問題への検出や対処、防止策の作成</li>
                                </ul>
                            </li>
                            <li>
                                <h5 class="font-weight-bold mt-1">データの転送 </h5>
                                <p>
                                    個人データを含むユーザーの情報は、データ保護法がお客様の管轄区域と異なる場合がある州、国またはその他の管轄区域外にあるコンピュータに転送され、維持管理されることがあります。
                                    国外に移住し、私たちに情報を提供することを選択した場合は、個人データを含むデータを国外に移管して処理する可能性があることに注意してください。
                                    この個人情報保護方針への同意と個人情報の提出は、その譲渡に対するユーザーの同意を表します。
                                    本サービスは、お客様のデータが本プライバシーポリシーに従って安全に取り扱われることを確実にするために必要なすべての措置を講じます。
                                </p>
                            </li>
                            <li>
                                <h5 class="font-weight-bold">データの開示</h5>
                                <p>本サービスは、お客様の個人情報を以下の目的で開示することがあります。 </p>
                                <ul pl-1>
                                    <li>法的義務の遵守、及び法的責任を守るため </li>
                                    <li>本サービスに関連する不正行為を防止または調査するため</li>
                                    <li>本サービスまたは一般のユーザーの個人的安全を守るため</li>
                                </ul>
                            </li>
                            <li>
                                <h5 class="font-weight-bold mt-1">データのセキュリティ </h5>
                                <p>あなたのデータのセキュリティは私たちにとって重要ですが、インターネット上での送信方法や電子記憶方法は100％安全ではありません。商業上許容される手段を使用してお客様の個人データを保護するよう努めていますが、絶対的な安全性を保証するものではありません。</p>
                            </li>
                            <li>
                                <h5 class="font-weight-bold">サービスプロバイダ </h5>
                                <p>
                                    本サービスは、本サービスの円滑化、本サービスを代行するサービスの提供、関連サービスの実施、またはサービスの使用方法の分析における本サービスの支援のために、第三者の企業および個人がデータを使用することがあります。
                                    これらの第三者は、私たちのためにこれらの作業を実行するためにのみあなたの個人データにアクセスし、他の目的のためにそれを開示または使用しないように義務付けられています。
                                </p>
                            </li>
                            <li>
                                <h5 class="font-weight-bold">他のサイトへのリンク </h5>
                                <p>
                                    当社のサービスには、当社が運営していない他のサイトへのリンクが含まれている場合があります。サードパーティのリンクをクリックすると、そのサードパーティのサイトに移動します。訪問するすべてのサイトのプライバシーポリシーを確認することを強くお勧めします。
                                    当社は、第三者のサイトまたはサービスのコンテンツ、プライバシーポリシー、または慣行を管理することはできません。
                                </p>
                            </li>
                            <li>
                                <h5 class="font-weight-bold">子供のプライバシー </h5>
                                <p>
                                    私たちのサービスは、18歳未満の人（「子供」）には対処していません。
                                    私たちは故意に18歳未満の方から個人情報を収集することはありません。あなたが親または保護者であり、あなたの子供が私たちに個人情報を提供していることをご存知の場合は、私たちに連絡してください。親の同意を確認せずに子供から個人データを収集したことに気がついた場合、その情報をサーバーから削除する手順を実行します。
                                </p>
                            </li>
                            <li>
                                <h5 class="font-weight-bold">このプライバシーポリシーの変更 </h5>
                                <p>当社は時々、当社のプライバシーポリシーを更新する可能性があります。このページに新しいプライバシーポリシーを掲載することにより、変更のお知らせをいたします。</p>
                            </li>
                            </ol>')
GO
INSERT [dbo].[MobileLangMsgDetail] ([Id], [LangId], [Name], [Value]) VALUES (4, 2, N'TermsOfService', N'この利用規約（以下、「本規約」といいます。）は、CRYPTOLOT（以下、「本サービス」といいます。）がCRYPTOLOTウェブサイト上（以下、「本ウェブサイト」といいます。）で提供するサービスの利用条件を定めるものです。登録ユーザーの皆さま（以下、「ユーザー」といいます。）には、本規約に従って本サービスをご利用いただきます。
                                <ol class="text-jutify pl-1">
                                    <li>
                                        <h5 class="font-weight-bold">適用について</h5>
                            <p>本規約は，ユーザーと本サービスの利用に関わる一切の関係に適用されるものとします。</p>
                            </li>
                            <li>
                                <h5 class="font-weight-bold">利用登録について</h5>
                                <ol pl-1>
                                    <li>登録希望者が本サービスの定める方法によって利用登録を申請し、本サービスがこれを承認することによって、利用登録が完了するものとします。</li>
                                    <li>本サービスは、利用登録の申請者に以下の事由があると判断した場合、利用登録の申請を承認しないことがあり、その理由については一切の開示義務を負わないものとします。</li>
                                    <ol pl-2>
                                        <li>利用登録の申請に際して虚偽の事項を届け出た場合</li>
                                        <li>本規約に違反したことがある者からの申請である場合</li>
                                        <li>その他，本サービスが利用登録を相当でないと判断した場合</li>
                                    </ol>
                                </ol>
                            </li>
                            <li>
                                <h5 class="font-weight-bold mt-1">ユーザーIDおよびパスワードの管理</h5>
                                <ol pl-1>
                                    <li>ユーザーは自己の責任において、本サービスのユーザーIDおよびパスワードを管理するものとします。 </li>
                                    <li>ユーザーはいかなる場合にも、ユーザーIDおよびパスワードを第三者に譲渡または貸与することはできません。本サービスは，ユーザーIDとパスワードの組み合わせが登録情報と一致してログインされた場合には，そのユーザーIDを登録しているユーザー自身による利用とみなします。 </li>
                                    <li>各プレイヤーは1つのアカウントのみを許可されます。多くのアカウントを持つユーザー、本サービスのシステムを妨害する目的でスクリプトなどを使用するプレーヤーは自動的にブロックされます。ブロックされたプレイヤーは、仮想通貨の出金を行うことはできません。 </li>
                                </ol>
                            </li>
                            <li>
                                <h5 class="font-weight-bold mt-1">トークンの管理</h5>
                                <ol pl-1>
                                    <li>本サービスのくじ購入で使用され、本サービス内で仮想通貨の交換等に使用されるトークンはCPL（以下トークン）と呼称されます。本規約中に記載されるトークンとは、このCPLを差し示します。</li>
                                    <li>本サービスは、6ヶ月以上継続して活動していないアカウントを閉鎖し、関連するトークンを保持する権利を留保しています。あなたのトークンは安全に保存されますが、長期保管はお勧めしません。また、トークンの交換に使用する仮想通貨を保管するためのデスクトップウォレットをダウンロードすることをお勧めします。</li>
                                    <li>ユーザーは本サービス利用の対価として、本サービスが別途定め本ウェブサイトに表示する利用レートに応じて、仮想通貨とトークンを交換します。また、その仮想通貨とトークンの交換は本サービスが指定する方法により行うものとします。</li>
                                </ol>
                            </li>
                            <li>
                                <h5 class="font-weight-bold mt-1">禁止事項</h5>
                                    <p>ユーザーは、本サービスの利用にあたり、以下の行為をしてはなりません。</p>
                                    <ol pl-1>
                                        <li>法令または公序良俗に違反する行為</li>
                                        <li>犯罪行為に関連する行為</li>
                                        <li>本サービスのサーバーまたはネットワークの機能を破壊したり，妨害したりする行為</li>
                                        <li>本サービスの運営を妨害するおそれのある行為</li>
                                        <li>他のユーザーに関する個人情報等を収集または蓄積する行為</li>
                                        <li>他のユーザーに成りすます行為</li>
                                        <li>本サービスに関連して，反社会的勢力に対して直接または間接に利益を供与する行為</li>
                                        <li>その他、本サービスが不適切と判断する行為</li>
                                    </ol>
                            </li>
                            <li>
                                <h5 class="font-weight-bold mt-1">本サービスの提供の停止等</h5>
                                <ol pl-1>
                                    <li>本サービスは、以下のいずれかの事由があると判断した場合，ユーザーに事前に通知することなく本サービスの全部または一部の提供を停止または中断することができるものとします。</li>
                                    <ol pl-2>
                                        <li>本サービスにかかるコンピュータシステムの保守点検または更新を行う場合</li>
                                        <li>地震、落雷、火災、停電または天災などの不可抗力により、本サービスの提供が困難となった場合</li>
                                        <li>コンピュータまたは通信回線等が事故により停止した場合</li>
                                        <li>その他、本サービスが本サービスの提供が困難と判断した場合</li>
                                    </ol>
                                    <li>本サービスは、本サービスの提供の停止または中断により、ユーザーまたは第三者が被ったいかなる不利益または損害について、理由を問わず一切の責任を負わないものとします。</li>
                                </ol>
                            </li>
                            <li>
                                <h5 class="font-weight-bold mt-1">利用制限および登録抹消</h5>
                                <ol pl-1>
                                    <li>本サービスは、事前に予告なくウェブサイト、サービス、ソフトウェアを変更したり、サービスにアクセスして使用するために必要なシステム仕様要件を変更する権利を留保します。</li>
                                    <li>本サービスは、以下の場合には事前の通知なく、ユーザーに対して、本サービスの全部もしくは一部の利用を制限し、またはユーザーとしての登録を抹消することができるものとします。</li>
                                    <ol pl-2>
                                        <li>本規約のいずれかの条項に違反した場合</li>
                                        <li>登録事項に虚偽の事実があることが判明した場合</li>
                                        <li>その他、本サービスが本サービスの利用を適当でないと判断した場合</li>
                                    </ol>
                                    <li>本サービスは、本条に基づき本サービスが行った行為によりユーザーに生じた損害について、一切の責任を負いません。</li>
                                    <li>18歳未満のユーザーによる本サービスの使用またはアクセスは禁止されています。 18歳未満の場合は、本サービスの情報を使用したり提供したりしないでください。</li>
                                    <li>本サービスは正当な理由により、拒否、サービスのキャンセル、および/または利益の分配を拒否する権利を独占的に留保します。</li>
                                </ol>
                            </li>
                            <li>
                                <h5 class="font-weight-bold mt-1">暗号通貨による入金について</h5>
                                <p>
                                    本サービスは現金または入金を受け付けていません。ビットコイン、イーサリアムのみが受け入れられます。
                                </p>
                            </li>
                            <li>
                                <h5 class="font-weight-bold">暗号通貨による出金について</h5>
                                <ol pl-1>
                                    <li>本サービスはビットコイン、イーサリアムによって出金を行います。その際、現金による出金はできません。また、出金の際には、ユーザーの身分証明をして頂く必要があります。</li>
                                    <li>ユーザーは、身分証明時に提供される情報の妥当性と有効性について責任を負います。</li>
                                    <li>本サービスは、ユーザーの身分証明の情報に虚偽がある場合、出金を制限、拒否する権利を有します。</li>
                                </ol>
                            </li>
                            <li>
                                <h5 class="font-weight-bold mt-1">免責事項</h5>
                                <ol pl-1>
                                    <li>本サービスは，ゲームの誤動作、オンラインシステム、サーバーの障害、誤動作、また、ユーザーによるくじ購入の結果、起こった損失による返金は一切行いません。また、賠償の責任も負いません。</li>
                                    <li>ゲームが誤動作した場合、誤動作期間中のすべてのゲームプレイは無効になります。</li>
                                    <li>ユーザーが不正行為をしたり、複数のアカウントを使用したり、出金を悪用したりすると、あなたのアカウントは閉鎖され、投資を含むあなたの仮想通貨及びトークンは没収されます。継続的に収益の引き出しは禁止され、ユーザーのアカウントはブロックされます。</li>
                                    <li>本サービスは、電話網や回線、コンピュータのオンラインシステム、サーバーまたはプロバイダ、ハードウェア、ソフトウェア、技術的な問題やインターネット、ウェブサイト、サービス上のトラフィック増大による不具合や技術的不具合については一切責任を負いません。</li>
                                    <li>本サービスは、データの入力ミス、データの保存と処理、送信されたデータの不完全性と不正確さに起因する損害については責任を負いません。</li>
                                    <li>本サービスは、本サービスに関するユーザーと他のユーザーまたは第三者との間において生じた取引，連絡または紛争等について一切責任を負いません。</li>
                                </ol>
                            </li>
                            <li>
                                <h5 class="font-weight-bold mt-1">サービス内容の変更等</h5>
                                <p>
                                    本サービスは、ユーザーに通知することなく、本サービスの内容を変更しまたは本サービスの提供を中止することができるものとし、これによってユーザーに生じた損害について一切の責任を負いません。
                                </p>
                            </li>
                            <li>
                                <h5 class="font-weight-bold">利用規約の変更</h5>
                                <p>
                                    本サービスは、必要と判断した場合には、ユーザーに通知することなくいつでも本規約を変更することができるものとします。
                                </p>
                            </li>
                            <li>
                                <h5 class="font-weight-bold">通知または連絡</h5>
                                <p>
                                    ユーザーと本サービスとの間の通知または連絡は、本サービスの定める方法によって行うものとします。
                                </p>
                            </li>
                            <li>
                                <h5 class="font-weight-bold">権利義務の譲渡の禁止</h5>
                                <p>
                                    ユーザーは、本サービスの書面による事前の承諾なく、利用契約上の地位または本規約に基づく権利もしくは義務を第三者に譲渡し、または担保に供することはできません。
                                </p>
                            </li>
                            <li>
                                <h5 class="font-weight-bold">準拠法・裁判管轄</h5>
                                <p>
                                    本サービスに関して紛争が生じた場合には、本サービスの本店所在地を管轄する裁判所を専属的合意管轄とします。
                                </p>
                            </li>
                            </ol>')
GO
SET IDENTITY_INSERT [dbo].[MobileLangMsgDetail] OFF
GO


INSERT INTO MobileLangDetail VALUES (1, N'ChoiceLanguageScreen_Title', N'Language Selection');
INSERT INTO MobileLangDetail VALUES (2, N'ChoiceLanguageScreen_Title', N'言語の選択');

INSERT INTO MobileLangDetail VALUES (1, N'ChoiceLanguageScreen_Button_Choice', N'Choose');
INSERT INTO MobileLangDetail VALUES (2, N'ChoiceLanguageScreen_Button_Choice', N'選択');

INSERT INTO MobileLangDetail VALUES (1, N'InitalScreen_Button_Login', N'Login');
INSERT INTO MobileLangDetail VALUES (2, N'InitalScreen_Button_Login', N'ログイン');

INSERT INTO MobileLangDetail VALUES (1, N'InitalScreen_Button_Register', N'Register');
INSERT INTO MobileLangDetail VALUES (2, N'InitalScreen_Button_Register', N'登録');

INSERT INTO MobileLangDetail VALUES (1, N'InitalScreen_Label_PrivacyPolicy', N'Privacy Policy');
INSERT INTO MobileLangDetail VALUES (2, N'InitalScreen_Label_PrivacyPolicy', N'プライバシーポリシー');

INSERT INTO MobileLangDetail VALUES (1, N'InitalScreen_Label_TermsOfService', N'Terms Of Service');
INSERT INTO MobileLangDetail VALUES (2, N'InitalScreen_Label_TermsOfService', N'利用規約');

INSERT INTO MobileLangDetail VALUES (1, N'InitalScreen_Label_Ampersand', N' & ');
INSERT INTO MobileLangDetail VALUES (2, N'InitalScreen_Label_Ampersand', N'　&　');

INSERT INTO MobileLangDetail VALUES (1, N'LoginScreen_Title', N'Please login with your account.');
INSERT INTO MobileLangDetail VALUES (2, N'LoginScreen_Title', N'あなたのログイン情報を入力してください。');

INSERT INTO MobileLangDetail VALUES (1, N'LoginScreen_Button_Login', N'Login');
INSERT INTO MobileLangDetail VALUES (2, N'LoginScreen_Button_Login', N'ログイン');

INSERT INTO MobileLangDetail VALUES (1, N'LoginScreen_Button_Register', N'Register');
INSERT INTO MobileLangDetail VALUES (2, N'LoginScreen_Button_Register', N'登録');

INSERT INTO MobileLangDetail VALUES (1, N'LoginScreen_Label_IfNoAccount', N'Have you created accounts? ');
INSERT INTO MobileLangDetail VALUES (2, N'LoginScreen_Label_IfNoAccount', N'アカウントを作成したことがありますか？ ');

INSERT INTO MobileLangDetail VALUES (1, N'LoginScreen_Input_Email', N'Email');
INSERT INTO MobileLangDetail VALUES (2, N'LoginScreen_Input_Email', N'メールアドレス');

INSERT INTO MobileLangDetail VALUES (1, N'LoginScreen_Input_Email_Required', N'Please fill in your email');
INSERT INTO MobileLangDetail VALUES (2, N'LoginScreen_Input_Email_Required', N'あなたメールアドレスを入力してください。');

INSERT INTO MobileLangDetail VALUES (1, N'LoginScreen_Input_Email_Invalid_Format', N'Please fill in your email');
INSERT INTO MobileLangDetail VALUES (2, N'LoginScreen_Input_Email_Invalid_Format', N'あなたメールアドレスを入力してください。');

INSERT INTO MobileLangDetail VALUES (1, N'LoginScreen_Input_Password', N'Password');
INSERT INTO MobileLangDetail VALUES (2, N'LoginScreen_Input_Password', N'パスワード');

INSERT INTO MobileLangDetail VALUES (1, N'LoginScreen_Input_Password_Required', N'Please fill in your password');
INSERT INTO MobileLangDetail VALUES (2, N'LoginScreen_Input_Password_Required', N'あなたのパスワードを入力してください。');

INSERT INTO MobileLangDetail VALUES (1, N'LoginScreen_Input_Password_Invalid_MinLength', N'Please fill in your password');
INSERT INTO MobileLangDetail VALUES (2, N'LoginScreen_Input_Password_Invalid_MinLength', N'あなたのパスワードを入力してください。');

INSERT INTO MobileLangDetail VALUES (1, N'RegisterScreen_Title', N'Please fill in your account information.');
INSERT INTO MobileLangDetail VALUES (2, N'RegisterScreen_Title', N'あなたのアカウント情報を入力してください。');

INSERT INTO MobileLangDetail VALUES (1, N'RegisterScreen_Button_Login', N'Login');
INSERT INTO MobileLangDetail VALUES (2, N'RegisterScreen_Button_Login', N'ログイン');

INSERT INTO MobileLangDetail VALUES (1, N'RegisterScreen_Button_Register', N'Register');
INSERT INTO MobileLangDetail VALUES (2, N'RegisterScreen_Button_Register', N'登録');

INSERT INTO MobileLangDetail VALUES (1, N'RegisterScreen_Label_IfCreatedAccount', N'Have you created accounts? ');
INSERT INTO MobileLangDetail VALUES (2, N'RegisterScreen_Label_IfCreatedAccount', N'アカウントを作成したことがありますか？ ');

INSERT INTO MobileLangDetail VALUES (1, N'RegisterScreen_Input_Email', N'Email');
INSERT INTO MobileLangDetail VALUES (2, N'RegisterScreen_Input_Email', N'メールアドレス');

INSERT INTO MobileLangDetail VALUES (1, N'RegisterScreen_Input_Email_Required', N'Please fill in your email');
INSERT INTO MobileLangDetail VALUES (2, N'RegisterScreen_Input_Email_Required', N'あなたメールアドレスを入力してください。');

INSERT INTO MobileLangDetail VALUES (1, N'RegisterScreen_Input_Email_Invalid_Format', N'Please fill in your email');
INSERT INTO MobileLangDetail VALUES (2, N'RegisterScreen_Input_Email_Invalid_Format', N'あなたメールアドレスを入力してください。');

INSERT INTO MobileLangDetail VALUES (1, N'RegisterScreen_Input_Password', N'Password');
INSERT INTO MobileLangDetail VALUES (2, N'RegisterScreen_Input_Password', N'パスワード');

INSERT INTO MobileLangDetail VALUES (1, N'RegisterScreen_Input_Password_Required', N'Please fill in your password');
INSERT INTO MobileLangDetail VALUES (2, N'RegisterScreen_Input_Password_Required', N'あなたのパスワードを入力してください。');

INSERT INTO MobileLangDetail VALUES (1, N'RegisterScreen_Input_Password_Invalid_MinLength', N'Please fill in your password');
INSERT INTO MobileLangDetail VALUES (2, N'RegisterScreen_Input_Password_Invalid_MinLength', N'あなたのパスワードを入力してください。');

INSERT INTO MobileLangDetail VALUES (1, N'RegisterScreen_Input_Confirmation_Password', N'Confirmation Password');
INSERT INTO MobileLangDetail VALUES (2, N'RegisterScreen_Input_Confirmation_Password', N'確認パスワード');

INSERT INTO MobileLangDetail VALUES (1, N'RegisterScreen_Input_Confirmation_Password_Required', N'Please fill in your confirmation password');
INSERT INTO MobileLangDetail VALUES (2, N'RegisterScreen_Input_Confirmation_Password_Required', N'あなたの確認パスワードを入力してください。');

INSERT INTO MobileLangDetail VALUES (1, N'RegisterScreen_Input_Confirmation_Password_Invalid_MinLength', N'Please fill in your confirmation password');
INSERT INTO MobileLangDetail VALUES (2, N'RegisterScreen_Input_Confirmation_Password_Invalid_MinLength', N'あなたの確認パスワードを入力してください。');

INSERT INTO MobileLangDetail VALUES (1, N'RegisterScreen_Input_Confirmation_Password_NoMatch', N'Mismatch password');
INSERT INTO MobileLangDetail VALUES (2, N'RegisterScreen_Input_Confirmation_Password_NoMatch', N'あなたの確認パスワードを入力してください。');

INSERT INTO MobileLangDetail VALUES (1, N'RegisterScreen_Input_Checkbox_Agree_Rules', N'I have read and agree ');
INSERT INTO MobileLangDetail VALUES (2, N'RegisterScreen_Input_Checkbox_Agree_Rules', N' に同意します');








