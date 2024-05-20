using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Persistence.Migrations
{
    public partial class aproverrepairs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Bio = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DataContrplTypes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CTName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DataContrplTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DataLines",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LineId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LineName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    isDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DataLines", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ErrorMessages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ErrorCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ErrorDescription = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ErrorMessages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ParameterChecks",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Order = table.Column<int>(type: "int", nullable: false),
                    ImageSampleUrl = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ParameterChecks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SComboBoxOptions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PCID = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OptionValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SComboBoxOptions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SelectOptions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PCID = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SOptionValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SelectOptions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TraceProducts",
                columns: table => new
                {
                    SerialNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StationName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RefreshToken",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AppUserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Token = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Expires = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Revoked = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefreshToken", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RefreshToken_AspNetUsers_AppUserId",
                        column: x => x.AppUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "WorkOrders",
                columns: table => new
                {
                    WoNumber = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SONumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WoReferenceID = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WoQTY = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PassQTY = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FailQTY = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WoStatus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WoCreate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserIdCreate = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    WOisDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkOrders", x => x.WoNumber);
                    table.ForeignKey(
                        name: "FK_WorkOrders_AspNetUsers_UserIdCreate",
                        column: x => x.UserIdCreate,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LastStationIDs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StationID = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StationName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LineId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LastStationIDs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LastStationIDs_DataLines_LineId",
                        column: x => x.LineId,
                        principalTable: "DataLines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ErrorTrack",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TrackPSN = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TrackingDateCreate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PCID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ErrorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ErrorTrack", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ErrorTrack_ErrorMessages_ErrorId",
                        column: x => x.ErrorId,
                        principalTable: "ErrorMessages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_ErrorTrack_ParameterChecks_PCID",
                        column: x => x.PCID,
                        principalTable: "ParameterChecks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ParameterCheckErrorMessages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ParameterCheckId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ErrorMessageId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Order = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ParameterCheckErrorMessages", x => new { x.Id, x.ParameterCheckId, x.ErrorMessageId });
                    table.ForeignKey(
                        name: "FK_ParameterCheckErrorMessages_ErrorMessages_ErrorMessageId",
                        column: x => x.ErrorMessageId,
                        principalTable: "ErrorMessages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ParameterCheckErrorMessages_ParameterChecks_ParameterCheckId",
                        column: x => x.ParameterCheckId,
                        principalTable: "ParameterChecks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DataReferences",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RefereceName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PsnPos = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RefPos = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RefCompare = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StationID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    isDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DataReferences", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DataReferences_LastStationIDs_StationID",
                        column: x => x.StationID,
                        principalTable: "LastStationIDs",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "DataTracks",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TrackPSN = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TrackReference = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TrackingWO = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TrackingLastStationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TrackingDateCreate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TrackingResult = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TrackingStatus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TrackingUserIdChecked = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ApprovalId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    DTisDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DataTracks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DataTracks_AspNetUsers_ApprovalId",
                        column: x => x.ApprovalId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DataTracks_AspNetUsers_TrackingUserIdChecked",
                        column: x => x.TrackingUserIdChecked,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DataTracks_LastStationIDs_TrackingLastStationId",
                        column: x => x.TrackingLastStationId,
                        principalTable: "LastStationIDs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DataReferenceParameterChecks",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DataReferenceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ParameterCheckId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Order = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DataReferenceParameterChecks", x => new { x.Id, x.DataReferenceId, x.ParameterCheckId });
                    table.ForeignKey(
                        name: "FK_DataReferenceParameterChecks_DataReferences_DataReferenceId",
                        column: x => x.DataReferenceId,
                        principalTable: "DataReferences",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DataReferenceParameterChecks_ParameterChecks_ParameterCheckId",
                        column: x => x.ParameterCheckId,
                        principalTable: "ParameterChecks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DataTrackCheckings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DataTrackID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PCID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DTCValue = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ErrorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Approve = table.Column<bool>(type: "bit", nullable: false),
                    ApprovalId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ApprRemaks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DTCisDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DataTrackCheckings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DataTrackCheckings_AspNetUsers_ApprovalId",
                        column: x => x.ApprovalId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DataTrackCheckings_DataTracks_DataTrackID",
                        column: x => x.DataTrackID,
                        principalTable: "DataTracks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DataTrackCheckings_ErrorMessages_ErrorId",
                        column: x => x.ErrorId,
                        principalTable: "ErrorMessages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DataTrackCheckings_ParameterChecks_PCID",
                        column: x => x.PCID,
                        principalTable: "ParameterChecks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ImageDataChecks",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DataTrackCheckingId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImageDataChecks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ImageDataChecks_DataTrackCheckings_DataTrackCheckingId",
                        column: x => x.DataTrackCheckingId,
                        principalTable: "DataTrackCheckings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_DataReferenceParameterChecks_DataReferenceId",
                table: "DataReferenceParameterChecks",
                column: "DataReferenceId");

            migrationBuilder.CreateIndex(
                name: "IX_DataReferenceParameterChecks_ParameterCheckId",
                table: "DataReferenceParameterChecks",
                column: "ParameterCheckId");

            migrationBuilder.CreateIndex(
                name: "IX_DataReferences_StationID",
                table: "DataReferences",
                column: "StationID");

            migrationBuilder.CreateIndex(
                name: "IX_DataTrackCheckings_ApprovalId",
                table: "DataTrackCheckings",
                column: "ApprovalId");

            migrationBuilder.CreateIndex(
                name: "IX_DataTrackCheckings_DataTrackID",
                table: "DataTrackCheckings",
                column: "DataTrackID");

            migrationBuilder.CreateIndex(
                name: "IX_DataTrackCheckings_ErrorId",
                table: "DataTrackCheckings",
                column: "ErrorId");

            migrationBuilder.CreateIndex(
                name: "IX_DataTrackCheckings_PCID",
                table: "DataTrackCheckings",
                column: "PCID");

            migrationBuilder.CreateIndex(
                name: "IX_DataTracks_ApprovalId",
                table: "DataTracks",
                column: "ApprovalId");

            migrationBuilder.CreateIndex(
                name: "IX_DataTracks_TrackingLastStationId",
                table: "DataTracks",
                column: "TrackingLastStationId");

            migrationBuilder.CreateIndex(
                name: "IX_DataTracks_TrackingUserIdChecked",
                table: "DataTracks",
                column: "TrackingUserIdChecked");

            migrationBuilder.CreateIndex(
                name: "IX_ErrorTrack_ErrorId",
                table: "ErrorTrack",
                column: "ErrorId");

            migrationBuilder.CreateIndex(
                name: "IX_ErrorTrack_PCID",
                table: "ErrorTrack",
                column: "PCID");

            migrationBuilder.CreateIndex(
                name: "IX_ImageDataChecks_DataTrackCheckingId",
                table: "ImageDataChecks",
                column: "DataTrackCheckingId");

            migrationBuilder.CreateIndex(
                name: "IX_LastStationIDs_LineId",
                table: "LastStationIDs",
                column: "LineId");

            migrationBuilder.CreateIndex(
                name: "IX_ParameterCheckErrorMessages_ErrorMessageId",
                table: "ParameterCheckErrorMessages",
                column: "ErrorMessageId");

            migrationBuilder.CreateIndex(
                name: "IX_ParameterCheckErrorMessages_ParameterCheckId",
                table: "ParameterCheckErrorMessages",
                column: "ParameterCheckId");

            migrationBuilder.CreateIndex(
                name: "IX_RefreshToken_AppUserId",
                table: "RefreshToken",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkOrders_UserIdCreate",
                table: "WorkOrders",
                column: "UserIdCreate");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "DataContrplTypes");

            migrationBuilder.DropTable(
                name: "DataReferenceParameterChecks");

            migrationBuilder.DropTable(
                name: "ErrorTrack");

            migrationBuilder.DropTable(
                name: "ImageDataChecks");

            migrationBuilder.DropTable(
                name: "ParameterCheckErrorMessages");

            migrationBuilder.DropTable(
                name: "RefreshToken");

            migrationBuilder.DropTable(
                name: "SComboBoxOptions");

            migrationBuilder.DropTable(
                name: "SelectOptions");

            migrationBuilder.DropTable(
                name: "TraceProducts");

            migrationBuilder.DropTable(
                name: "WorkOrders");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "DataReferences");

            migrationBuilder.DropTable(
                name: "DataTrackCheckings");

            migrationBuilder.DropTable(
                name: "DataTracks");

            migrationBuilder.DropTable(
                name: "ErrorMessages");

            migrationBuilder.DropTable(
                name: "ParameterChecks");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "LastStationIDs");

            migrationBuilder.DropTable(
                name: "DataLines");
        }
    }
}
