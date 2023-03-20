using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CIWeb.Migrations
{
    /// <inheritdoc />
    public partial class first_db : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "admin",
                columns: table => new
                {
                    admin_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    first_name = table.Column<string>(type: "varchar(16)", unicode: false, maxLength: 16, nullable: true),
                    last_name = table.Column<string>(type: "varchar(16)", unicode: false, maxLength: 16, nullable: true),
                    email = table.Column<string>(type: "varchar(128)", unicode: false, maxLength: 128, nullable: false),
                    password = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    updated_at = table.Column<DateTime>(type: "datetime", nullable: true),
                    deleted_at = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__admin__43AA4141C62062DC", x => x.admin_id);
                });

            migrationBuilder.CreateTable(
                name: "banner",
                columns: table => new
                {
                    banner_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    image = table.Column<string>(type: "varchar(512)", unicode: false, maxLength: 512, nullable: false),
                    text = table.Column<string>(type: "text", nullable: true),
                    sort_order = table.Column<int>(type: "int", nullable: true, defaultValueSql: "((0))"),
                    created_at = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    updated_at = table.Column<DateTime>(type: "datetime", nullable: true),
                    deleted_at = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__banner__10373C342E1231E7", x => x.banner_id);
                });

            migrationBuilder.CreateTable(
                name: "cms_page",
                columns: table => new
                {
                    cms_page_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    title = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    description = table.Column<string>(type: "text", nullable: true),
                    slug = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    status = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true, defaultValueSql: "((1))"),
                    created_at = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    updated_at = table.Column<DateTime>(type: "datetime", nullable: true),
                    deleted_at = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__cms_page__B46D5B527292911B", x => x.cms_page_id);
                });

            migrationBuilder.CreateTable(
                name: "country",
                columns: table => new
                {
                    country_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    iso = table.Column<string>(type: "varchar(16)", unicode: false, maxLength: 16, nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    updated_at = table.Column<DateTime>(type: "datetime", nullable: true),
                    deleted_at = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__country__7E8CD05594670E4B", x => x.country_id);
                });

            migrationBuilder.CreateTable(
                name: "mission_theme",
                columns: table => new
                {
                    mission_theme_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    title = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    status = table.Column<byte>(type: "tinyint", nullable: false, defaultValueSql: "((1))"),
                    created_at = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    updated_at = table.Column<DateTime>(type: "datetime", nullable: true),
                    deleted_at = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__mission___4925C5AC3529762B", x => x.mission_theme_id);
                });

            migrationBuilder.CreateTable(
                name: "password_reset",
                columns: table => new
                {
                    email = table.Column<string>(type: "nvarchar(191)", maxLength: 191, nullable: true),
                    token = table.Column<string>(type: "nvarchar(191)", maxLength: 191, nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "skill",
                columns: table => new
                {
                    skill_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    skill_name = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    status = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false, defaultValueSql: "((1))"),
                    created_at = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    updated_at = table.Column<DateTime>(type: "datetime", nullable: true),
                    deleted_at = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__skill__FBBA8379C3ADD75D", x => x.skill_id);
                });

            migrationBuilder.CreateTable(
                name: "city",
                columns: table => new
                {
                    city_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    country_id = table.Column<long>(type: "bigint", nullable: false),
                    name = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    updated_at = table.Column<DateTime>(type: "datetime", nullable: true),
                    deleted_at = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__city__031491A8A16C1EE4", x => x.city_id);
                    table.ForeignKey(
                        name: "FK__city__country_id__3C69FB99",
                        column: x => x.country_id,
                        principalTable: "country",
                        principalColumn: "country_id");
                });

            migrationBuilder.CreateTable(
                name: "mission",
                columns: table => new
                {
                    mission_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    city_id = table.Column<long>(type: "bigint", nullable: false),
                    country_id = table.Column<long>(type: "bigint", nullable: false),
                    theme_id = table.Column<long>(type: "bigint", nullable: false),
                    title = table.Column<string>(type: "varchar(128)", unicode: false, maxLength: 128, nullable: false),
                    short_description = table.Column<string>(type: "text", nullable: true),
                    description = table.Column<string>(type: "text", nullable: true),
                    start_date = table.Column<DateTime>(type: "datetime", nullable: true),
                    end_date = table.Column<DateTime>(type: "datetime", nullable: true),
                    mission_type = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false),
                    status = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true),
                    organization_name = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    organization_detail = table.Column<string>(type: "text", nullable: true),
                    availability = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    updated_at = table.Column<DateTime>(type: "datetime", nullable: true),
                    deleted_at = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__mission__B5419AB2C32661E7", x => x.mission_id);
                    table.ForeignKey(
                        name: "FK__mission__city_id__6477ECF3",
                        column: x => x.city_id,
                        principalTable: "city",
                        principalColumn: "city_id");
                    table.ForeignKey(
                        name: "FK__mission__country__656C112C",
                        column: x => x.country_id,
                        principalTable: "country",
                        principalColumn: "country_id");
                    table.ForeignKey(
                        name: "FK__mission__theme_i__66603565",
                        column: x => x.theme_id,
                        principalTable: "mission_theme",
                        principalColumn: "mission_theme_id");
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    user_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    first_name = table.Column<string>(type: "varchar(16)", unicode: false, maxLength: 16, nullable: true),
                    last_name = table.Column<string>(type: "varchar(16)", unicode: false, maxLength: 16, nullable: true),
                    email = table.Column<string>(type: "varchar(128)", unicode: false, maxLength: 128, nullable: false),
                    password = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    phone_number = table.Column<int>(type: "int", nullable: false),
                    avatar = table.Column<string>(type: "varchar(2048)", unicode: false, maxLength: 2048, nullable: true),
                    why_i_volunteer = table.Column<string>(type: "text", nullable: true),
                    employee_id = table.Column<string>(type: "varchar(16)", unicode: false, maxLength: 16, nullable: true),
                    department = table.Column<string>(type: "varchar(16)", unicode: false, maxLength: 16, nullable: true),
                    city_id = table.Column<long>(type: "bigint", nullable: false),
                    country_id = table.Column<long>(type: "bigint", nullable: false),
                    profile_text = table.Column<string>(type: "text", nullable: true),
                    linked_in_url = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    title = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    status = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false, defaultValueSql: "((1))"),
                    created_at = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    updated_at = table.Column<DateTime>(type: "datetime", nullable: true),
                    deleted_at = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__users__B9BE370FE735EC63", x => x.user_id);
                    table.ForeignKey(
                        name: "FK__users__city_id__47DBAE45",
                        column: x => x.city_id,
                        principalTable: "city",
                        principalColumn: "city_id");
                    table.ForeignKey(
                        name: "FK__users__country_i__48CFD27E",
                        column: x => x.country_id,
                        principalTable: "country",
                        principalColumn: "country_id");
                });

            migrationBuilder.CreateTable(
                name: "goal_mission",
                columns: table => new
                {
                    goal_mission_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    mission_id = table.Column<long>(type: "bigint", nullable: false),
                    goal_objective_text = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    goal_value = table.Column<string>(type: "varchar(11)", unicode: false, maxLength: 11, nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    updated_at = table.Column<DateTime>(type: "datetime", nullable: true),
                    deleted_at = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__goal_mis__358E02C7CAC34D0E", x => x.goal_mission_id);
                    table.ForeignKey(
                        name: "FK__goal_miss__missi__05D8E0BE",
                        column: x => x.mission_id,
                        principalTable: "mission",
                        principalColumn: "mission_id");
                });

            migrationBuilder.CreateTable(
                name: "mission_document",
                columns: table => new
                {
                    mission_document_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    mission_id = table.Column<long>(type: "bigint", nullable: false),
                    document_name = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    document_type = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    document_path = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    updated_at = table.Column<DateTime>(type: "datetime", nullable: true),
                    deleted_at = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__mission___E80E0CC89B678847", x => x.mission_document_id);
                    table.ForeignKey(
                        name: "FK__mission_d__missi__14270015",
                        column: x => x.mission_id,
                        principalTable: "mission",
                        principalColumn: "mission_id");
                });

            migrationBuilder.CreateTable(
                name: "mission_media",
                columns: table => new
                {
                    mission_media_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    mission_id = table.Column<long>(type: "bigint", nullable: false),
                    media_name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    media_type = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    media_path = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    default_ = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true, defaultValueSql: "((0))"),
                    created_at = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    updated_at = table.Column<DateTime>(type: "datetime", nullable: true),
                    deleted_at = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__mission___848A78E8E0B91848", x => x.mission_media_id);
                    table.ForeignKey(
                        name: "FK__mission_m__missi__2180FB33",
                        column: x => x.mission_id,
                        principalTable: "mission",
                        principalColumn: "mission_id");
                });

            migrationBuilder.CreateTable(
                name: "mission_skill",
                columns: table => new
                {
                    mission_skill_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    skill_id = table.Column<long>(type: "bigint", nullable: false),
                    mission_id = table.Column<long>(type: "bigint", nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    updated_at = table.Column<DateTime>(type: "datetime", nullable: true),
                    deleted_at = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__mission___8271200849820C08", x => x.mission_skill_id);
                    table.ForeignKey(
                        name: "FK__mission_s__missi__3587F3E0",
                        column: x => x.mission_id,
                        principalTable: "mission",
                        principalColumn: "mission_id");
                    table.ForeignKey(
                        name: "FK__mission_s__skill__3493CFA7",
                        column: x => x.skill_id,
                        principalTable: "skill",
                        principalColumn: "skill_id");
                });

            migrationBuilder.CreateTable(
                name: "comment",
                columns: table => new
                {
                    comment_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    user_id = table.Column<long>(type: "bigint", nullable: true),
                    mission_id = table.Column<long>(type: "bigint", nullable: false),
                    approval_status = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false, defaultValueSql: "('pending')"),
                    created_at = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    updated_at = table.Column<DateTime>(type: "datetime", nullable: true),
                    deleted_at = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__comment__E7957687B158C918", x => x.comment_id);
                    table.ForeignKey(
                        name: "FK__comment__mission__72C60C4A",
                        column: x => x.mission_id,
                        principalTable: "mission",
                        principalColumn: "mission_id");
                    table.ForeignKey(
                        name: "FK__comment__user_id__71D1E811",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "user_id");
                });

            migrationBuilder.CreateTable(
                name: "favorite_mission",
                columns: table => new
                {
                    favourite_mission_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    user_id = table.Column<long>(type: "bigint", nullable: true),
                    mission_id = table.Column<long>(type: "bigint", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    updated_at = table.Column<DateTime>(type: "datetime", nullable: true),
                    deleted_at = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__favorite__94E4D8CAEAD00364", x => x.favourite_mission_id);
                    table.ForeignKey(
                        name: "FK__favorite___missi__00200768",
                        column: x => x.mission_id,
                        principalTable: "mission",
                        principalColumn: "mission_id");
                    table.ForeignKey(
                        name: "FK__favorite___user___7F2BE32F",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "user_id");
                });

            migrationBuilder.CreateTable(
                name: "mission_application",
                columns: table => new
                {
                    mission_application_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    mission_id = table.Column<long>(type: "bigint", nullable: false),
                    user_id = table.Column<long>(type: "bigint", nullable: false),
                    applied_at = table.Column<DateTime>(type: "datetime", nullable: false),
                    approval_status = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false, defaultValueSql: "('pending')"),
                    created_at = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    updated_at = table.Column<DateTime>(type: "datetime", nullable: true),
                    deleted_at = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__mission___DF92838AED1FAB6A", x => x.mission_application_id);
                    table.ForeignKey(
                        name: "FK__mission_a__missi__0C85DE4D",
                        column: x => x.mission_id,
                        principalTable: "mission",
                        principalColumn: "mission_id");
                    table.ForeignKey(
                        name: "FK__mission_a__user___0D7A0286",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "user_id");
                });

            migrationBuilder.CreateTable(
                name: "mission_invite",
                columns: table => new
                {
                    mission_invite_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    mission_id = table.Column<long>(type: "bigint", nullable: false),
                    from_user_id = table.Column<long>(type: "bigint", nullable: false),
                    to_user_id = table.Column<long>(type: "bigint", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    updated_at = table.Column<DateTime>(type: "datetime", nullable: true),
                    deleted_at = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__mission___A97ED158B048A706", x => x.mission_invite_id);
                    table.ForeignKey(
                        name: "FK__mission_i__from___1AD3FDA4",
                        column: x => x.from_user_id,
                        principalTable: "users",
                        principalColumn: "user_id");
                    table.ForeignKey(
                        name: "FK__mission_i__missi__19DFD96B",
                        column: x => x.mission_id,
                        principalTable: "mission",
                        principalColumn: "mission_id");
                    table.ForeignKey(
                        name: "FK__mission_i__to_us__1BC821DD",
                        column: x => x.to_user_id,
                        principalTable: "users",
                        principalColumn: "user_id");
                });

            migrationBuilder.CreateTable(
                name: "mission_rating",
                columns: table => new
                {
                    mission_rating_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    mission_id = table.Column<long>(type: "bigint", nullable: false),
                    user_id = table.Column<long>(type: "bigint", nullable: false),
                    rating = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    updated_at = table.Column<DateTime>(type: "datetime", nullable: true),
                    deleted_at = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__mission___320E517204FA845F", x => x.mission_rating_id);
                    table.ForeignKey(
                        name: "FK__mission_r__missi__282DF8C2",
                        column: x => x.mission_id,
                        principalTable: "mission",
                        principalColumn: "mission_id");
                    table.ForeignKey(
                        name: "FK__mission_r__user___29221CFB",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "user_id");
                });

            migrationBuilder.CreateTable(
                name: "story",
                columns: table => new
                {
                    story_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    mission_id = table.Column<long>(type: "bigint", nullable: false),
                    user_id = table.Column<long>(type: "bigint", nullable: false),
                    title = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    description = table.Column<string>(type: "text", nullable: true),
                    status = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true, defaultValueSql: "('draft')"),
                    published_at = table.Column<DateTime>(type: "datetime", nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    updated_at = table.Column<DateTime>(type: "datetime", nullable: true),
                    deleted_at = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__story__66339C5636E973D0", x => x.story_id);
                    table.ForeignKey(
                        name: "FK__story__mission_i__3E1D39E1",
                        column: x => x.mission_id,
                        principalTable: "mission",
                        principalColumn: "mission_id");
                    table.ForeignKey(
                        name: "FK__story__user_id__3F115E1A",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "user_id");
                });

            migrationBuilder.CreateTable(
                name: "timesheet",
                columns: table => new
                {
                    timesheet_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    mission_id = table.Column<long>(type: "bigint", nullable: true),
                    user_id = table.Column<long>(type: "bigint", nullable: true),
                    timesheet_time = table.Column<TimeSpan>(type: "time", nullable: true),
                    action = table.Column<int>(type: "int", nullable: true),
                    date_volunteered = table.Column<DateTime>(type: "datetime", nullable: false),
                    notes = table.Column<string>(type: "text", nullable: true),
                    status = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false, defaultValueSql: "('pending')"),
                    created_at = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    updated_at = table.Column<DateTime>(type: "datetime", nullable: true),
                    deleted_at = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__timeshee__7BBF5068B66D089A", x => x.timesheet_id);
                    table.ForeignKey(
                        name: "FK__timesheet__missi__55F4C372",
                        column: x => x.mission_id,
                        principalTable: "mission",
                        principalColumn: "mission_id");
                    table.ForeignKey(
                        name: "FK__timesheet__user___56E8E7AB",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "user_id");
                });

            migrationBuilder.CreateTable(
                name: "user_skill",
                columns: table => new
                {
                    user_skill_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    skill_id = table.Column<long>(type: "bigint", nullable: false),
                    user_id = table.Column<long>(type: "bigint", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    updated_at = table.Column<DateTime>(type: "datetime", nullable: true),
                    deleted_at = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__user_ski__FD3B576B2A735A6A", x => x.user_skill_id);
                    table.ForeignKey(
                        name: "FK__user_skil__skill__5D95E53A",
                        column: x => x.skill_id,
                        principalTable: "skill",
                        principalColumn: "skill_id");
                    table.ForeignKey(
                        name: "FK__user_skil__user___5E8A0973",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "user_id");
                });

            migrationBuilder.CreateTable(
                name: "story_invite",
                columns: table => new
                {
                    story_invite_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    story_id = table.Column<long>(type: "bigint", nullable: false),
                    from_user_id = table.Column<long>(type: "bigint", nullable: false),
                    to_user_id = table.Column<long>(type: "bigint", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    updated_at = table.Column<DateTime>(type: "datetime", nullable: true),
                    deleted_at = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__story_in__04497867E8BA21BD", x => x.story_invite_id);
                    table.ForeignKey(
                        name: "FK__story_inv__from___498EEC8D",
                        column: x => x.from_user_id,
                        principalTable: "users",
                        principalColumn: "user_id");
                    table.ForeignKey(
                        name: "FK__story_inv__story__489AC854",
                        column: x => x.story_id,
                        principalTable: "story",
                        principalColumn: "story_id");
                    table.ForeignKey(
                        name: "FK__story_inv__to_us__4A8310C6",
                        column: x => x.to_user_id,
                        principalTable: "users",
                        principalColumn: "user_id");
                });

            migrationBuilder.CreateTable(
                name: "story_media",
                columns: table => new
                {
                    story_media_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    story_id = table.Column<long>(type: "bigint", nullable: false),
                    story_type = table.Column<string>(type: "varchar(8)", unicode: false, maxLength: 8, nullable: false),
                    story_path = table.Column<string>(type: "text", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    updated_at = table.Column<DateTime>(type: "datetime", nullable: true),
                    deleted_at = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__story_me__29BD053C9518EB78", x => x.story_media_id);
                    table.ForeignKey(
                        name: "FK__story_med__story__503BEA1C",
                        column: x => x.story_id,
                        principalTable: "story",
                        principalColumn: "story_id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_city_country_id",
                table: "city",
                column: "country_id");

            migrationBuilder.CreateIndex(
                name: "IX_comment_mission_id",
                table: "comment",
                column: "mission_id");

            migrationBuilder.CreateIndex(
                name: "IX_comment_user_id",
                table: "comment",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_favorite_mission_mission_id",
                table: "favorite_mission",
                column: "mission_id");

            migrationBuilder.CreateIndex(
                name: "IX_favorite_mission_user_id",
                table: "favorite_mission",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_goal_mission_mission_id",
                table: "goal_mission",
                column: "mission_id");

            migrationBuilder.CreateIndex(
                name: "IX_mission_city_id",
                table: "mission",
                column: "city_id");

            migrationBuilder.CreateIndex(
                name: "IX_mission_country_id",
                table: "mission",
                column: "country_id");

            migrationBuilder.CreateIndex(
                name: "IX_mission_theme_id",
                table: "mission",
                column: "theme_id");

            migrationBuilder.CreateIndex(
                name: "IX_mission_application_mission_id",
                table: "mission_application",
                column: "mission_id");

            migrationBuilder.CreateIndex(
                name: "IX_mission_application_user_id",
                table: "mission_application",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_mission_document_mission_id",
                table: "mission_document",
                column: "mission_id");

            migrationBuilder.CreateIndex(
                name: "IX_mission_invite_from_user_id",
                table: "mission_invite",
                column: "from_user_id");

            migrationBuilder.CreateIndex(
                name: "IX_mission_invite_mission_id",
                table: "mission_invite",
                column: "mission_id");

            migrationBuilder.CreateIndex(
                name: "IX_mission_invite_to_user_id",
                table: "mission_invite",
                column: "to_user_id");

            migrationBuilder.CreateIndex(
                name: "IX_mission_media_mission_id",
                table: "mission_media",
                column: "mission_id");

            migrationBuilder.CreateIndex(
                name: "IX_mission_rating_mission_id",
                table: "mission_rating",
                column: "mission_id");

            migrationBuilder.CreateIndex(
                name: "IX_mission_rating_user_id",
                table: "mission_rating",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_mission_skill_mission_id",
                table: "mission_skill",
                column: "mission_id");

            migrationBuilder.CreateIndex(
                name: "IX_mission_skill_skill_id",
                table: "mission_skill",
                column: "skill_id");

            migrationBuilder.CreateIndex(
                name: "IX_story_mission_id",
                table: "story",
                column: "mission_id");

            migrationBuilder.CreateIndex(
                name: "IX_story_user_id",
                table: "story",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_story_invite_from_user_id",
                table: "story_invite",
                column: "from_user_id");

            migrationBuilder.CreateIndex(
                name: "IX_story_invite_story_id",
                table: "story_invite",
                column: "story_id");

            migrationBuilder.CreateIndex(
                name: "IX_story_invite_to_user_id",
                table: "story_invite",
                column: "to_user_id");

            migrationBuilder.CreateIndex(
                name: "IX_story_media_story_id",
                table: "story_media",
                column: "story_id");

            migrationBuilder.CreateIndex(
                name: "IX_timesheet_mission_id",
                table: "timesheet",
                column: "mission_id");

            migrationBuilder.CreateIndex(
                name: "IX_timesheet_user_id",
                table: "timesheet",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_user_skill_skill_id",
                table: "user_skill",
                column: "skill_id");

            migrationBuilder.CreateIndex(
                name: "IX_user_skill_user_id",
                table: "user_skill",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_users_city_id",
                table: "users",
                column: "city_id");

            migrationBuilder.CreateIndex(
                name: "IX_users_country_id",
                table: "users",
                column: "country_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "admin");

            migrationBuilder.DropTable(
                name: "banner");

            migrationBuilder.DropTable(
                name: "cms_page");

            migrationBuilder.DropTable(
                name: "comment");

            migrationBuilder.DropTable(
                name: "favorite_mission");

            migrationBuilder.DropTable(
                name: "goal_mission");

            migrationBuilder.DropTable(
                name: "mission_application");

            migrationBuilder.DropTable(
                name: "mission_document");

            migrationBuilder.DropTable(
                name: "mission_invite");

            migrationBuilder.DropTable(
                name: "mission_media");

            migrationBuilder.DropTable(
                name: "mission_rating");

            migrationBuilder.DropTable(
                name: "mission_skill");

            migrationBuilder.DropTable(
                name: "password_reset");

            migrationBuilder.DropTable(
                name: "story_invite");

            migrationBuilder.DropTable(
                name: "story_media");

            migrationBuilder.DropTable(
                name: "timesheet");

            migrationBuilder.DropTable(
                name: "user_skill");

            migrationBuilder.DropTable(
                name: "story");

            migrationBuilder.DropTable(
                name: "skill");

            migrationBuilder.DropTable(
                name: "mission");

            migrationBuilder.DropTable(
                name: "users");

            migrationBuilder.DropTable(
                name: "mission_theme");

            migrationBuilder.DropTable(
                name: "city");

            migrationBuilder.DropTable(
                name: "country");
        }
    }
}
