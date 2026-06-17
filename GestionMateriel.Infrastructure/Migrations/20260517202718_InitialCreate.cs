using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace GestionMateriel.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "features",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    slug = table.Column<string>(type: "character varying(100)", unicode: false, maxLength: 100, nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_features", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "structures",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    code_structure = table.Column<string>(type: "character varying(50)", unicode: false, maxLength: 50, nullable: false),
                    nom_structure = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    type = table.Column<string>(type: "character varying(20)", unicode: false, maxLength: 20, nullable: false),
                    parent_code = table.Column<string>(type: "character varying(50)", unicode: false, maxLength: 50, nullable: true),
                    color = table.Column<string>(type: "character varying(7)", unicode: false, maxLength: 7, nullable: true),
                    image = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_structures", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    firstname = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    lastname = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    email = table.Column<string>(type: "character varying(255)", unicode: false, maxLength: 255, nullable: false),
                    password = table.Column<string>(type: "text", nullable: false),
                    phone = table.Column<string>(type: "character varying(20)", unicode: false, maxLength: 20, nullable: true),
                    role = table.Column<string>(type: "character varying(20)", unicode: false, maxLength: 20, nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "item_categories",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    structure_id = table.Column<int>(type: "integer", nullable: false),
                    identified = table.Column<bool>(type: "boolean", nullable: false),
                    code_structure = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_item_categories", x => x.id);
                    table.ForeignKey(
                        name: "FK_item_categories_structures_structure_id",
                        column: x => x.structure_id,
                        principalTable: "structures",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "events",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    start_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    end_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    structure_id = table.Column<int>(type: "integer", nullable: false),
                    user_id = table.Column<int>(type: "integer", nullable: true),
                    comment = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    code_structure = table.Column<string>(type: "text", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_events", x => x.id);
                    table.ForeignKey(
                        name: "FK_events_structures_structure_id",
                        column: x => x.structure_id,
                        principalTable: "structures",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_events_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "feature_clicks",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    feature_id = table.Column<int>(type: "integer", nullable: false),
                    user_id = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_feature_clicks", x => x.id);
                    table.ForeignKey(
                        name: "FK_feature_clicks_features_feature_id",
                        column: x => x.feature_id,
                        principalTable: "features",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_feature_clicks_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "refresh_tokens",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    token = table.Column<string>(type: "text", nullable: false),
                    user_id = table.Column<int>(type: "integer", nullable: false),
                    expires_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_refresh_tokens", x => x.id);
                    table.ForeignKey(
                        name: "FK_refresh_tokens_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "user_structures",
                columns: table => new
                {
                    user_id = table.Column<int>(type: "integer", nullable: false),
                    structure_id = table.Column<int>(type: "integer", nullable: false),
                    role = table.Column<string>(type: "character varying(20)", unicode: false, maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_structures", x => new { x.user_id, x.structure_id });
                    table.ForeignKey(
                        name: "FK_user_structures_structures_structure_id",
                        column: x => x.structure_id,
                        principalTable: "structures",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_user_structures_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "items",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    category_id = table.Column<int>(type: "integer", nullable: false),
                    structure_id = table.Column<int>(type: "integer", nullable: false),
                    usable = table.Column<bool>(type: "boolean", nullable: false),
                    stock = table.Column<int>(type: "integer", nullable: false),
                    date_of_buy = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    image = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    code_structure = table.Column<string>(type: "character varying(50)", unicode: false, maxLength: 50, nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_items", x => x.id);
                    table.ForeignKey(
                        name: "FK_items_item_categories_category_id",
                        column: x => x.category_id,
                        principalTable: "item_categories",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_items_structures_structure_id",
                        column: x => x.structure_id,
                        principalTable: "structures",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "event_subscriptions",
                columns: table => new
                {
                    event_id = table.Column<int>(type: "integer", nullable: false),
                    item_id = table.Column<int>(type: "integer", nullable: false),
                    quantity = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_event_subscriptions", x => new { x.event_id, x.item_id });
                    table.ForeignKey(
                        name: "FK_event_subscriptions_events_event_id",
                        column: x => x.event_id,
                        principalTable: "events",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_event_subscriptions_items_item_id",
                        column: x => x.item_id,
                        principalTable: "items",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "item_issues",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    item_id = table.Column<int>(type: "integer", nullable: false),
                    status = table.Column<string>(type: "character varying(20)", unicode: false, maxLength: 20, nullable: false),
                    value = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    reported_by = table.Column<int>(type: "integer", nullable: false),
                    affected_quantity = table.Column<int>(type: "integer", nullable: false),
                    resolution_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_item_issues", x => x.id);
                    table.ForeignKey(
                        name: "FK_item_issues_items_item_id",
                        column: x => x.item_id,
                        principalTable: "items",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_item_issues_users_reported_by",
                        column: x => x.reported_by,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "item_issue_comments",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    item_issue_id = table.Column<int>(type: "integer", nullable: false),
                    comment = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    user_id = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_item_issue_comments", x => x.id);
                    table.ForeignKey(
                        name: "FK_item_issue_comments_item_issues_item_issue_id",
                        column: x => x.item_issue_id,
                        principalTable: "item_issues",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_item_issue_comments_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_event_subscriptions_item_id",
                table: "event_subscriptions",
                column: "item_id");

            migrationBuilder.CreateIndex(
                name: "IX_events_structure_id",
                table: "events",
                column: "structure_id");

            migrationBuilder.CreateIndex(
                name: "IX_events_user_id",
                table: "events",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_feature_clicks_feature_id",
                table: "feature_clicks",
                column: "feature_id");

            migrationBuilder.CreateIndex(
                name: "IX_feature_clicks_user_id",
                table: "feature_clicks",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_features_slug",
                table: "features",
                column: "slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_item_categories_structure_id",
                table: "item_categories",
                column: "structure_id");

            migrationBuilder.CreateIndex(
                name: "IX_item_issue_comments_item_issue_id",
                table: "item_issue_comments",
                column: "item_issue_id");

            migrationBuilder.CreateIndex(
                name: "IX_item_issue_comments_user_id",
                table: "item_issue_comments",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_item_issues_item_id",
                table: "item_issues",
                column: "item_id");

            migrationBuilder.CreateIndex(
                name: "IX_item_issues_reported_by",
                table: "item_issues",
                column: "reported_by");

            migrationBuilder.CreateIndex(
                name: "IX_items_category_id",
                table: "items",
                column: "category_id");

            migrationBuilder.CreateIndex(
                name: "IX_items_structure_id",
                table: "items",
                column: "structure_id");

            migrationBuilder.CreateIndex(
                name: "IX_refresh_tokens_user_id",
                table: "refresh_tokens",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_user_structures_structure_id",
                table: "user_structures",
                column: "structure_id");

            migrationBuilder.CreateIndex(
                name: "IX_users_email",
                table: "users",
                column: "email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "event_subscriptions");

            migrationBuilder.DropTable(
                name: "feature_clicks");

            migrationBuilder.DropTable(
                name: "item_issue_comments");

            migrationBuilder.DropTable(
                name: "refresh_tokens");

            migrationBuilder.DropTable(
                name: "user_structures");

            migrationBuilder.DropTable(
                name: "events");

            migrationBuilder.DropTable(
                name: "features");

            migrationBuilder.DropTable(
                name: "item_issues");

            migrationBuilder.DropTable(
                name: "items");

            migrationBuilder.DropTable(
                name: "users");

            migrationBuilder.DropTable(
                name: "item_categories");

            migrationBuilder.DropTable(
                name: "structures");
        }
    }
}
