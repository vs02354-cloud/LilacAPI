--
-- PostgreSQL database dump
--

-- Dumped from database version 16.2
-- Dumped by pg_dump version 16.2

SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SELECT pg_catalog.set_config('search_path', '', false);
SET check_function_bodies = false;
SET xmloption = content;
SET client_min_messages = warning;
SET row_security = off;

ALTER TABLE IF EXISTS ONLY public."Projects" DROP CONSTRAINT IF EXISTS "FK_Projects_Categories_CategoryId";
ALTER TABLE IF EXISTS ONLY public."JobApplications" DROP CONSTRAINT IF EXISTS "FK_JobApplications_JobOpenings_JobOpeningId";
ALTER TABLE IF EXISTS ONLY public."BlogPosts" DROP CONSTRAINT IF EXISTS "FK_BlogPosts_Categories_CategoryId";
DROP INDEX IF EXISTS public."IX_Subscribers_Email";
DROP INDEX IF EXISTS public."IX_Services_Slug";
DROP INDEX IF EXISTS public."IX_Projects_Slug";
DROP INDEX IF EXISTS public."IX_Projects_CategoryId";
DROP INDEX IF EXISTS public."IX_JobOpenings_Slug";
DROP INDEX IF EXISTS public."IX_JobApplications_JobOpeningId";
DROP INDEX IF EXISTS public."IX_Categories_Slug";
DROP INDEX IF EXISTS public."IX_BlogPosts_Slug";
DROP INDEX IF EXISTS public."IX_BlogPosts_CategoryId";
DROP INDEX IF EXISTS public."IX_AdminUsers_Username";
DROP INDEX IF EXISTS public."IX_AdminUsers_Email";
ALTER TABLE IF EXISTS ONLY public."Testimonials" DROP CONSTRAINT IF EXISTS "PK_Testimonials";
ALTER TABLE IF EXISTS ONLY public."TeamMembers" DROP CONSTRAINT IF EXISTS "PK_TeamMembers";
ALTER TABLE IF EXISTS ONLY public."Subscribers" DROP CONSTRAINT IF EXISTS "PK_Subscribers";
ALTER TABLE IF EXISTS ONLY public."Services" DROP CONSTRAINT IF EXISTS "PK_Services";
ALTER TABLE IF EXISTS ONLY public."QuoteRequests" DROP CONSTRAINT IF EXISTS "PK_QuoteRequests";
ALTER TABLE IF EXISTS ONLY public."Projects" DROP CONSTRAINT IF EXISTS "PK_Projects";
ALTER TABLE IF EXISTS ONLY public."JobOpenings" DROP CONSTRAINT IF EXISTS "PK_JobOpenings";
ALTER TABLE IF EXISTS ONLY public."JobApplications" DROP CONSTRAINT IF EXISTS "PK_JobApplications";
ALTER TABLE IF EXISTS ONLY public."ContactMessages" DROP CONSTRAINT IF EXISTS "PK_ContactMessages";
ALTER TABLE IF EXISTS ONLY public."Categories" DROP CONSTRAINT IF EXISTS "PK_Categories";
ALTER TABLE IF EXISTS ONLY public."BlogPosts" DROP CONSTRAINT IF EXISTS "PK_BlogPosts";
ALTER TABLE IF EXISTS ONLY public."AdminUsers" DROP CONSTRAINT IF EXISTS "PK_AdminUsers";
DROP TABLE IF EXISTS public."Testimonials";
DROP TABLE IF EXISTS public."TeamMembers";
DROP TABLE IF EXISTS public."Subscribers";
DROP TABLE IF EXISTS public."Services";
DROP TABLE IF EXISTS public."QuoteRequests";
DROP TABLE IF EXISTS public."Projects";
DROP TABLE IF EXISTS public."JobOpenings";
DROP TABLE IF EXISTS public."JobApplications";
DROP TABLE IF EXISTS public."ContactMessages";
DROP TABLE IF EXISTS public."Categories";
DROP TABLE IF EXISTS public."BlogPosts";
DROP TABLE IF EXISTS public."AdminUsers";
SET default_tablespace = '';

SET default_table_access_method = heap;

--
-- Name: AdminUsers; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public."AdminUsers" (
    "Id" uuid NOT NULL,
    "Username" character varying(100) NOT NULL,
    "Email" character varying(255) NOT NULL,
    "PasswordHash" text NOT NULL,
    "FullName" character varying(150) NOT NULL,
    "Role" integer NOT NULL,
    "RefreshToken" text,
    "RefreshTokenExpiry" timestamp with time zone,
    "LastLoginAt" timestamp with time zone,
    "IsActive" boolean NOT NULL,
    "CreatedAt" timestamp with time zone NOT NULL,
    "UpdatedAt" timestamp with time zone
);


--
-- Name: BlogPosts; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public."BlogPosts" (
    "Id" uuid NOT NULL,
    "Title" character varying(300) NOT NULL,
    "Slug" character varying(300) NOT NULL,
    "Excerpt" text NOT NULL,
    "ContentHtml" text NOT NULL,
    "CoverImageUrl" text NOT NULL,
    "AuthorName" character varying(150) NOT NULL,
    "AuthorRole" text NOT NULL,
    "AuthorAvatarUrl" text,
    "CategoryId" uuid NOT NULL,
    "TagsJson" text NOT NULL,
    "ReadTimeMinutes" integer NOT NULL,
    "ViewCount" integer NOT NULL,
    "IsPublished" boolean NOT NULL,
    "PublishedAt" timestamp with time zone,
    "CreatedAt" timestamp with time zone NOT NULL,
    "UpdatedAt" timestamp with time zone
);


--
-- Name: Categories; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public."Categories" (
    "Id" uuid NOT NULL,
    "Name" character varying(150) NOT NULL,
    "Slug" character varying(150) NOT NULL,
    "Description" text NOT NULL,
    "DisplayOrder" integer NOT NULL,
    "CreatedAt" timestamp with time zone NOT NULL,
    "UpdatedAt" timestamp with time zone
);


--
-- Name: ContactMessages; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public."ContactMessages" (
    "Id" uuid NOT NULL,
    "FullName" character varying(150) NOT NULL,
    "Email" character varying(255) NOT NULL,
    "Phone" character varying(50),
    "Subject" character varying(250) NOT NULL,
    "Message" text NOT NULL,
    "IsRead" boolean NOT NULL,
    "RespondedAt" timestamp with time zone,
    "ResponseNotes" text,
    "CreatedAt" timestamp with time zone NOT NULL,
    "UpdatedAt" timestamp with time zone
);


--
-- Name: JobApplications; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public."JobApplications" (
    "Id" uuid NOT NULL,
    "JobOpeningId" uuid NOT NULL,
    "ApplicantName" character varying(150) NOT NULL,
    "Email" character varying(255) NOT NULL,
    "Phone" character varying(50) NOT NULL,
    "ResumeFileName" text NOT NULL,
    "ResumeFilePath" character varying(500) NOT NULL,
    "CoverLetter" text,
    "PortfolioUrl" text,
    "Status" integer NOT NULL,
    "AdminNotes" text,
    "CreatedAt" timestamp with time zone NOT NULL,
    "UpdatedAt" timestamp with time zone
);


--
-- Name: JobOpenings; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public."JobOpenings" (
    "Id" uuid NOT NULL,
    "Title" character varying(200) NOT NULL,
    "Slug" character varying(200) NOT NULL,
    "Department" text NOT NULL,
    "Location" text NOT NULL,
    "Type" text NOT NULL,
    "ExperienceLevel" text NOT NULL,
    "Description" text NOT NULL,
    "RequirementsJson" text NOT NULL,
    "ResponsibilitiesJson" text NOT NULL,
    "BenefitsJson" text NOT NULL,
    "IsActive" boolean NOT NULL,
    "Deadline" timestamp with time zone,
    "CreatedAt" timestamp with time zone NOT NULL,
    "UpdatedAt" timestamp with time zone
);


--
-- Name: Projects; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public."Projects" (
    "Id" uuid NOT NULL,
    "Title" character varying(250) NOT NULL,
    "Slug" character varying(250) NOT NULL,
    "ClientName" character varying(200) NOT NULL,
    "Summary" text NOT NULL,
    "FullDescription" text NOT NULL,
    "Challenge" text NOT NULL,
    "Solution" text NOT NULL,
    "ResultsJson" text NOT NULL,
    "ThumbnailUrl" text NOT NULL,
    "BannerUrl" text NOT NULL,
    "GalleryJson" text NOT NULL,
    "TechStackJson" text NOT NULL,
    "ProjectUrl" text,
    "CategoryId" uuid NOT NULL,
    "IsFeatured" boolean NOT NULL,
    "DisplayOrder" integer NOT NULL,
    "CreatedAt" timestamp with time zone NOT NULL,
    "UpdatedAt" timestamp with time zone
);


--
-- Name: QuoteRequests; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public."QuoteRequests" (
    "Id" uuid NOT NULL,
    "FullName" character varying(150) NOT NULL,
    "Email" character varying(255) NOT NULL,
    "Phone" character varying(50),
    "Company" character varying(150),
    "ServiceRequired" text NOT NULL,
    "BudgetRange" text NOT NULL,
    "Timeline" text NOT NULL,
    "ProjectDescription" text NOT NULL,
    "Status" integer NOT NULL,
    "AdminNotes" text,
    "EstimatedQuoteAmount" numeric,
    "CreatedAt" timestamp with time zone NOT NULL,
    "UpdatedAt" timestamp with time zone
);


--
-- Name: Services; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public."Services" (
    "Id" uuid NOT NULL,
    "Title" character varying(200) NOT NULL,
    "Slug" character varying(200) NOT NULL,
    "ShortDescription" text NOT NULL,
    "DetailedDescription" text NOT NULL,
    "Icon" character varying(100) NOT NULL,
    "FeaturesJson" text NOT NULL,
    "BenefitsJson" text NOT NULL,
    "TechnologiesJson" text NOT NULL,
    "DisplayOrder" integer NOT NULL,
    "IsActive" boolean NOT NULL,
    "CreatedAt" timestamp with time zone NOT NULL,
    "UpdatedAt" timestamp with time zone
);


--
-- Name: Subscribers; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public."Subscribers" (
    "Id" uuid NOT NULL,
    "Email" character varying(255) NOT NULL,
    "IsActive" boolean NOT NULL,
    "SubscribedAt" timestamp with time zone NOT NULL,
    "CreatedAt" timestamp with time zone NOT NULL,
    "UpdatedAt" timestamp with time zone
);


--
-- Name: TeamMembers; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public."TeamMembers" (
    "Id" uuid NOT NULL,
    "FullName" character varying(150) NOT NULL,
    "Role" character varying(150) NOT NULL,
    "Bio" text NOT NULL,
    "AvatarUrl" text NOT NULL,
    "Department" character varying(100) NOT NULL,
    "LinkedInUrl" text,
    "TwitterUrl" text,
    "GithubUrl" text,
    "DisplayOrder" integer NOT NULL,
    "IsActive" boolean NOT NULL,
    "CreatedAt" timestamp with time zone NOT NULL,
    "UpdatedAt" timestamp with time zone
);


--
-- Name: Testimonials; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public."Testimonials" (
    "Id" uuid NOT NULL,
    "ClientName" character varying(150) NOT NULL,
    "ClientTitle" text NOT NULL,
    "CompanyName" character varying(150) NOT NULL,
    "AvatarUrl" text NOT NULL,
    "Rating" integer NOT NULL,
    "Content" text NOT NULL,
    "ProjectName" text NOT NULL,
    "IsFeatured" boolean NOT NULL,
    "DisplayOrder" integer NOT NULL,
    "CreatedAt" timestamp with time zone NOT NULL,
    "UpdatedAt" timestamp with time zone
);


--
-- Data for Name: AdminUsers; Type: TABLE DATA; Schema: public; Owner: -
--

INSERT INTO public."AdminUsers" VALUES ('c6db3fc0-af7f-4c82-824c-f79c3301dadd', 'admin', 'admin@lilactechsys.com', '$2a$11$R7CdwKY8CdxwOKeJcCqd4uOBiVptJzYcXMnI7gsq/S4iq0F3Rde32', 'Lilac Admin', 2, 'ubKEjdzXrLUgoirbCjeDCHmPfMcNgX0LUHXGW9UZMekvcU1Ik957+I+rbKfjcrJE2CCrHk75hYic2xIcLC04CA==', '2026-10-09 13:29:44.848585+05:30', '2026-10-02 13:29:44.848586+05:30', true, '2026-09-30 21:40:37.626022+05:30', '2026-10-02 13:29:44.848587+05:30');


--
-- Data for Name: BlogPosts; Type: TABLE DATA; Schema: public; Owner: -
--

INSERT INTO public."BlogPosts" VALUES ('d25b9f0e-f27c-441d-aaed-c982ce72e1d6', 'Why Modern Enterprises Are Shifting to Headless Web Architecture', 'why-enterprises-shift-to-headless-web', 'Separating frontend presentation from backend logic delivers faster speed, superior omnichannel reach, and enhanced security.', '<p>Coupled monolithic CMS platforms once ruled the web. Today, omnichannel demands—from mobile apps to smart kiosks and lightning-fast web storefronts—demand API-first decoupled architectures.</p>
<h3>Benefits of Headless Systems</h3>
<p>By consuming structured REST or GraphQL APIs, frontend engineers can leverage modern component frameworks like React and Vite to deliver instantaneous sub-second user interactions without waiting on legacy server render cycles.</p>', 'https://images.unsplash.com/photo-1460925895917-afdab827c52f?auto=format&fit=crop&w=800&q=80', 'Sophia Vance', 'Chief Design Officer', NULL, '22222222-2222-2222-2222-222222222222', '["Web Dev","Headless","React","Performance"]', 5, 385, true, '2026-09-28 21:40:38.631063+05:30', '2026-09-30 21:40:38.631056+05:30', NULL);
INSERT INTO public."BlogPosts" VALUES ('fecc815e-049c-4872-8bbf-1948a0c66cfd', 'The Zero-Trust Security Paradigm: Practical Enterprise Guide', 'zero-trust-security-paradigm-guide', 'Moving past legacy VPNs into continuous identity verification, device attestation, and least-privilege access models.', '<p>The traditional castle-and-moat security perimeter is obsolete. Modern workforces are hybrid, workloads live in multi-cloud clusters, and attacks originate from compromised user credentials.</p>
<h3>Never Trust, Always Verify</h3>
<p>Zero Trust enforces strict identity validation for every person and device attempting to access network resources, regardless of whether they sit inside or outside the corporate intranet.</p>
<h3>Core Tenets of Zero Trust</h3>
<ul>
<li><strong>Explicit Identity Verification:</strong> Leverage multi-factor authentication (MFA) backed by FIDO2 keys.</li>
<li><strong>Least Privilege Access:</strong> Just-in-time access tokens with strict time-to-live boundaries.</li>
<li><strong>Assume Breach:</strong> Segment network micro-perimeters so lateral movement by bad actors is blocked.</li>
</ul>', 'https://images.unsplash.com/photo-1550751827-4bd374c3f58b?auto=format&fit=crop&w=800&q=80', 'Dr. Arvind Patel', 'CEO & Tech Strategist', NULL, '44444444-4444-4444-4444-444444444444', '["Cybersecurity","Zero-Trust","Compliance","Security"]', 8, 612, true, '2026-09-23 21:40:38.631055+05:30', '2026-09-30 21:40:38.631038+05:30', NULL);
INSERT INTO public."BlogPosts" VALUES ('7af13f9f-74af-4ccf-856b-ae0e898d1b5e', 'Architecting Resilient Cloud-Native Microservices in 2026', 'architecting-resilient-cloud-native-microservices', 'How distributed caching, circuit breakers, and container orchestration ensure fault-tolerant systems under enterprise load.', '<p>In modern enterprise engineering, resilience is not an accidental feature—it is an architectural discipline. Monolithic failures cascaded unchecked; distributed systems must isolate faults gracefully.</p>
<h3>1. The Circuit Breaker Pattern</h3>
<p>When an upstream dependency suffers elevated latency or errors, cascading pool exhaustion can bring down unrelated sub-systems. Utilizing Polly in .NET Core or Resilience4j creates adaptive circuit breakers that trip into fallback responses within milliseconds.</p>
<h3>2. Eventual Consistency with Outbox Patterns</h3>
<p>Distributed transactions across microservices introduce brittle two-phase locks. Implementing the Transactional Outbox Pattern ensures state changes and Kafka events are committed atomically inside your local PostgreSQL database, eliminating message loss.</p>
<h3>3. Observability Over Mere Monitoring</h3>
<p>Modern Kubernetes deployments require OpenTelemetry distributed tracing to map request spans end-to-end across multiple container boundaries.</p>', 'https://images.unsplash.com/photo-1451187580459-43490279c0fa?auto=format&fit=crop&w=800&q=80', 'Liam O''Connor', 'Head of Cloud Architecture', NULL, '11111111-1111-1111-1111-111111111111', '["Microservices","Cloud","Kubernetes","Architecture"]', 6, 430, true, '2026-09-16 21:40:38.630947+05:30', '2026-09-30 21:40:38.630365+05:30', NULL);


--
-- Data for Name: Categories; Type: TABLE DATA; Schema: public; Owner: -
--

INSERT INTO public."Categories" VALUES ('11111111-1111-1111-1111-111111111111', 'Cloud & DevOps', 'cloud-devops', 'Enterprise cloud migration, Kubernetes orchestration, and continuous delivery.', 1, '2026-09-30 21:40:37.82221+05:30', NULL);
INSERT INTO public."Categories" VALUES ('22222222-2222-2222-2222-222222222222', 'Enterprise Web & SaaS', 'enterprise-web-saas', 'Scalable web applications, customer portals, and mission-critical SaaS platforms.', 2, '2026-09-30 21:40:37.822522+05:30', NULL);
INSERT INTO public."Categories" VALUES ('33333333-3333-3333-3333-333333333333', 'Mobile & Cross-Platform', 'mobile-apps', 'High-performance iOS, Android, and cross-platform Flutter/React Native solutions.', 3, '2026-09-30 21:40:37.822528+05:30', NULL);
INSERT INTO public."Categories" VALUES ('44444444-4444-4444-4444-444444444444', 'Cybersecurity & Governance', 'cybersecurity', 'Zero-trust architecture, threat defense, compliance, and vulnerability assessments.', 4, '2026-09-30 21:40:37.822528+05:30', NULL);


--
-- Data for Name: ContactMessages; Type: TABLE DATA; Schema: public; Owner: -
--

INSERT INTO public."ContactMessages" VALUES ('fe6be05c-401c-49ab-a9bc-4faa404b27f7', 'Jane Doe', 'jane.doe@enterprise.org', '+1987654321', 'Enterprise Security Audit', 'We need an architectural review of our payment gateway.', false, NULL, NULL, '2026-10-02 13:29:45.451421+05:30', NULL);


--
-- Data for Name: JobApplications; Type: TABLE DATA; Schema: public; Owner: -
--



--
-- Data for Name: JobOpenings; Type: TABLE DATA; Schema: public; Owner: -
--

INSERT INTO public."JobOpenings" VALUES ('0b2208a1-ccee-419f-b112-8f673ac52e97', 'Cloud & DevOps Solutions Architect', 'cloud-devops-solutions-architect', 'Cloud & Security', 'Hybrid / Remote', 'Full-time', 'Lead (7+ years)', 'Lead the design and implementation of automated, self-healing cloud infrastructure and zero-downtime deployment pipelines.', '["Extensive experience with AWS or Azure cloud architectures and Kubernetes","Proficiency in Terraform, Helm, and GitOps workflows (ArgoCD)","Deep knowledge of security standards (SOC 2, ISO 27001, Zero-Trust)"]', '["Define cloud architecture standards across all client engagements","Automate multi-region failover and infrastructure monitoring","Conduct architecture reviews and optimize cloud infrastructure costs"]', '["Executive level compensation package","Comprehensive health, vision, and dental insurance","Unlimited PTO policy with mandatory minimum rest days"]', true, '2026-10-30 21:40:38.90247+05:30', '2026-09-30 21:40:38.902449+05:30', NULL);
INSERT INTO public."JobOpenings" VALUES ('30c2aba8-6d72-412b-ab4f-a9c4cc1a94f3', 'Senior Full-Stack .NET & React Engineer', 'senior-full-stack-dotnet-react-engineer', 'Engineering', 'Remote (Global)', 'Full-time', 'Senior (5+ years)', 'We are seeking an experienced full-stack engineer to lead the development of enterprise cloud applications using ASP.NET Core (.NET 9) and React.js.', '["5\u002B years professional experience building web applications in C# / .NET Core and React","Deep understanding of relational databases (PostgreSQL/SQL Server) and query optimization","Familiarity with containerized environments (Docker, Kubernetes) and CI/CD pipelines","Strong communication skills and passion for clean, readable code and unit testing"]', '["Architect scalable backend APIs and high-performance frontend interfaces","Lead code reviews, design docs, and architectural alignment sessions","Collaborate closely with UI/UX designers and product managers"]', '["Competitive compensation with annual performance bonus","Flexible 100% remote working culture","$2,500 annual personal learning \u0026 tech conference stipend","Comprehensive health and wellness coverage"]', true, '2026-11-14 21:40:38.902358+05:30', '2026-09-30 21:40:38.899923+05:30', NULL);
INSERT INTO public."JobOpenings" VALUES ('6e298360-f283-4d53-9744-d6e2e38538a0', 'Lead UI/UX Product Designer', 'lead-ui-ux-product-designer', 'Product & Design', 'Remote', 'Full-time', 'Senior (4+ years)', 'Craft world-class user interfaces and interactive experiences for cutting-edge SaaS, fintech, and healthcare solutions.', '["Portfolio demonstrating high aesthetic polish, typography mastery, and responsive design","Expertise in Figma design systems, tokens, and auto-layout","Knowledge of WCAG accessibility standards and micro-interaction design"]', '["Own end-to-end design from user flows and wireframes to pixel-perfect design systems","Collaborate with frontend developers on animation and interaction fidelity","Facilitate client design workshops and prototype evaluations"]', '["Generous home office stipend for top-tier hardware and displays","Collaborative, design-first engineering environment","Annual wellness and fitness allowance"]', true, '2026-11-29 21:40:38.902483+05:30', '2026-09-30 21:40:38.902471+05:30', NULL);


--
-- Data for Name: Projects; Type: TABLE DATA; Schema: public; Owner: -
--

INSERT INTO public."Projects" VALUES ('69e2bedf-b971-4b43-abd1-c4daf1cc0b5b', 'ZeroGuard Enterprise Access & Security Mesh', 'zeroguard-security-mesh', 'DefenSys Corporation', 'Implemented a zero-trust software-defined perimeter protecting 8,000 corporate devices from insider and external threats.', 'DefenSys required a complete migration from brittle corporate VPNs to an identity-aware proxy architecture with granular conditional access and continuous posture verification.', 'Remote work expansion overloaded legacy VPN concentrators, resulting in security blind spots and cumbersome user access friction.', 'Implemented an OAuth2/OIDC identity proxy with device compliance posture checking, automated revocation triggers, and a unified employee self-service portal.', '[{"metric":"0","label":"Perimeter Breaches"},{"metric":"70%","label":"Drop in Helpdesk Tickets"},{"metric":"8,000","label":"Secured Endpoints"}]', 'https://images.unsplash.com/photo-1563986768609-322da13575f3?auto=format&fit=crop&w=800&q=80', 'https://images.unsplash.com/photo-1563986768609-322da13575f3?auto=format&fit=crop&w=1600&q=80', '["https://images.unsplash.com/photo-1563986768609-322da13575f3?auto=format\u0026fit=crop\u0026w=800\u0026q=80"]', '["Zero-Trust","ASP.NET Core","PostgreSQL","React","Docker"]', NULL, '44444444-4444-4444-4444-444444444444', true, 4, '2026-09-30 21:40:38.469187+05:30', NULL);
INSERT INTO public."Projects" VALUES ('6e41f13c-8d27-4b65-a262-131cf1312dab', 'ApexHealth EHR & Telemedicine Engine', 'apexhealth-ehr-telemedicine', 'Apex Health Network', 'HIPAA-compliant telemedicine engine connecting 120,000+ patients with certified healthcare providers.', 'Apex Health Network required a seamless, high-security telemedicine portal with encrypted WebRTC video visits, automated medical transcription, and real-time electronic health records synchronization.', 'Fragmented EHR systems caused double-entry for doctors and unacceptable appointment drop-offs due to video codec incompatibilities on mobile browsers.', 'Built a unified WebRTC-powered portal with end-to-end encrypted medical data pipelines and automated physician workflow tools built with React, ASP.NET Core, and WebSockets.', '[{"metric":"120K\u002B","label":"Active Patients"},{"metric":"4.9/5","label":"Provider Rating"},{"metric":"100%","label":"HIPAA Compliance"}]', 'https://images.unsplash.com/photo-1576091160399-112ba8d25d1d?auto=format&fit=crop&w=800&q=80', 'https://images.unsplash.com/photo-1576091160399-112ba8d25d1d?auto=format&fit=crop&w=1600&q=80', '["https://images.unsplash.com/photo-1576091160399-112ba8d25d1d?auto=format\u0026fit=crop\u0026w=800\u0026q=80"]', '["React.js","WebRTC",".NET 9","PostgreSQL","Tailwind CSS"]', 'https://apexhealth.example.com', '33333333-3333-3333-3333-333333333333', true, 2, '2026-09-30 21:40:38.469079+05:30', NULL);
INSERT INTO public."Projects" VALUES ('963a44a4-a3b0-4b37-a126-d5c8b5f05743', 'AuraPay Global Banking & Settlement Platform', 'aurapay-global-platform', 'Aura Financial Group', 'Modernized cross-border payments infrastructure processing over $40M daily with sub-second clearing.', 'Aura Financial Group faced severe concurrency bottlenecks on legacy payment gateways. LilacTechSys engineered a modular cloud-native settlement platform incorporating event-driven microservices and localized payment adapters.', 'Legacy monolithic architecture suffered from 4.2-second average transaction latency and intermittent timeouts during peak European settlement hours.', 'Architected an event-driven ASP.NET Core 9 and PostgreSQL microservice cluster with distributed Redis caching, Kafka transaction queues, and a responsive React management portal.', '[{"metric":"82%","label":"Reduction in Latency"},{"metric":"99.995%","label":"System Uptime"},{"metric":"$40M\u002B","label":"Daily Settlement Volume"}]', 'https://images.unsplash.com/photo-1559526324-4b87b5e36e44?auto=format&fit=crop&w=800&q=80', 'https://images.unsplash.com/photo-1559526324-4b87b5e36e44?auto=format&fit=crop&w=1600&q=80', '["https://images.unsplash.com/photo-1559526324-4b87b5e36e44?auto=format\u0026fit=crop\u0026w=800\u0026q=80","https://images.unsplash.com/photo-1551288049-bebda4e38f71?auto=format\u0026fit=crop\u0026w=800\u0026q=80"]', '["React.js","ASP.NET Core","PostgreSQL","Kafka","Docker","Tailwind CSS"]', 'https://aurapay.example.com', '22222222-2222-2222-2222-222222222222', true, 1, '2026-09-30 21:40:38.44326+05:30', NULL);
INSERT INTO public."Projects" VALUES ('ce5064b3-ac65-4205-a935-258a8adcd73f', 'OmniSupply Real-Time IoT Logistics Mesh', 'omnisupply-iot-logistics', 'OmniSupply Global', 'Fleet telemetry and warehouse automation tracking over 35,000 cross-continental shipments in real time.', 'OmniSupply needed real-time visibility into temperature-sensitive pharmaceutical shipments with predictive delivery alerts and automated customs clearance documentation.', 'Unreliable cellular coverage during transit resulted in lost telemetry and missed temperature spike alerts for vaccine cargo.', 'Designed an edge-computing gateway with store-and-forward telemetry, cloud event streaming, and an intuitive live operations map built with React and Mapbox.', '[{"metric":"35K\u002B","label":"Tracked Cargo Units"},{"metric":"-34%","label":"Spoilage Incidents"},{"metric":"99.8%","label":"Telemetry Accuracy"}]', 'https://images.unsplash.com/photo-1586528116311-ad8dd3c8310d?auto=format&fit=crop&w=800&q=80', 'https://images.unsplash.com/photo-1586528116311-ad8dd3c8310d?auto=format&fit=crop&w=1600&q=80', '["https://images.unsplash.com/photo-1586528116311-ad8dd3c8310d?auto=format\u0026fit=crop\u0026w=800\u0026q=80"]', '["React","TypeScript","ASP.NET Core","TimescaleDB","Kubernetes"]', NULL, '11111111-1111-1111-1111-111111111111', true, 3, '2026-09-30 21:40:38.469155+05:30', NULL);


--
-- Data for Name: QuoteRequests; Type: TABLE DATA; Schema: public; Owner: -
--

INSERT INTO public."QuoteRequests" VALUES ('32566be0-a033-430d-9519-963a3a1887fc', 'Test Client', 'test@company.com', '+1234567890', NULL, '', '', '', '', 1, NULL, NULL, '2026-10-02 13:29:45.009168+05:30', NULL);


--
-- Data for Name: Services; Type: TABLE DATA; Schema: public; Owner: -
--

INSERT INTO public."Services" VALUES ('35ebdf1f-778e-4966-ac92-2a91084a81fe', 'Maintenance & 24/7 Support', 'maintenance-and-support', 'Dedicated SLAs, preventative monitoring, security patching, and ongoing performance tuning.', 'Keep your software reliable, fast, and secure post-launch. Our dedicated site reliability engineers provide SLA-backed monitoring, bug fixes, routine dependency upgrades, and ongoing cloud cost optimizations.', 'Headphones', '["24/7/365 Incident Response \u0026 Uptime SLAs","Scheduled Security Patching \u0026 Library Upgrades","Real-Time APM Monitoring \u0026 Log Diagnostics","Continuous Performance \u0026 Database Tuning"]', '["Guaranteed 15-minute response SLA for critical incidents","Elimination of unexpected software decay and bugs","Predictable monthly operational budgets"]', '["Datadog","Prometheus","Grafana","Sentry","PagerDuty"]', 8, true, '2026-09-30 21:40:38.369407+05:30', NULL);
INSERT INTO public."Services" VALUES ('3a5d7c80-52b9-4966-9d4c-2561327e4723', 'Mobile App Development', 'mobile-app-development', 'Fluid, native and cross-platform mobile apps for iOS and Android that elevate customer engagement.', 'Transform ideas into frictionless mobile touchpoints. We build consumer-facing and enterprise mobile applications utilizing React Native, Flutter, and native iOS/Android codebases that provide 60fps performance and secure offline capabilities.', 'Smartphone', '["Cross-Platform iOS \u0026 Android Apps","Offline First Architecture \u0026 Data Sync","Biometric Authentication \u0026 Hardware Integration","App Store Optimization (ASO) \u0026 Deployment"]', '["Single codebase reducing engineering overhead by up to 40%","Consistent design system matching brand identity","Instant push notifications and real-time interaction"]', '["React Native","Flutter","Swift","Kotlin","Firebase","WebSockets"]', 2, true, '2026-09-30 21:40:38.369253+05:30', NULL);
INSERT INTO public."Services" VALUES ('59dfae3d-547b-4074-9a7d-6e3b3c749bd3', 'Cloud & DevOps', 'cloud-and-devops', 'Automated CI/CD pipelines, Kubernetes container orchestration, and multi-cloud resilience.', 'Accelerate delivery cadences with enterprise DevOps. We engineer immutable infrastructure-as-code, self-healing Kubernetes clusters, and zero-downtime deployment pipelines across AWS, Azure, and Google Cloud.', 'Cloud', '["Infrastructure as Code (Terraform, Pulumi)","Kubernetes Cluster Deployment \u0026 GitOps (ArgoCD)","Automated CI/CD Pipelines (GitHub Actions, GitLab)","Multi-Region Failover \u0026 Disaster Recovery"]', '["Daily release cadence with automated quality gates","99.99% infrastructure uptime with auto-scaling","30%\u002B reduction in unnecessary cloud resource spend"]', '["AWS","Azure","Docker","Kubernetes","Terraform","GitHub Actions"]', 4, true, '2026-09-30 21:40:38.369308+05:30', NULL);
INSERT INTO public."Services" VALUES ('92531cbf-c3a7-4d29-957e-3b5db0bdc602', 'Cybersecurity & Compliance', 'cybersecurity', 'Zero-trust defenses, continuous vulnerability monitoring, penetration testing, and compliance readiness.', 'Safeguard intellectual property and sensitive customer data. We implement zero-trust network architectures, perform vulnerability assessments, and ensure compliance readiness across ISO 27001, SOC 2, and GDPR.', 'ShieldCheck', '["Zero-Trust Architecture \u0026 IAM Governance","Automated Vulnerability Scanning \u0026 Pentesting","SOC 2, ISO 27001 \u0026 GDPR Compliance Frameworks","Incident Response Strategy \u0026 Threat Modeling"]', '["Proactive mitigation of breach and ransom vulnerabilities","Frictionless customer trust and audit readiness","24/7 endpoint visibility and rapid threat containment"]', '["Zero-Trust","OAuth2 / OIDC","WAF","SonarQube","OWASP Top 10"]', 7, true, '2026-09-30 21:40:38.369389+05:30', NULL);
INSERT INTO public."Services" VALUES ('a2f3f401-ae96-486a-80e5-64dc705f533e', 'IT Consulting & Advisory', 'it-consulting', 'Strategic technology roadmapping, enterprise architecture design, and CTO-level guidance.', 'Navigate complex technology investments with clarity. Our seasoned enterprise architects audit existing stacks, identify technical debt, and formulate phased modernization roadmaps aligned directly with revenue goals.', 'Briefcase', '["Digital Transformation Roadmaps","Enterprise Architecture \u0026 Stack Audits","Cloud Migration Planning \u0026 TCO Analysis","Fractional CTO \u0026 Technical Due Diligence"]', '["De-risked technology modernization initiatives","Direct alignment between IT expenditure and business value","Objective vendor evaluation free of commercial bias"]', '["Enterprise Architecture","TOGAF","Cloud Economics","Microservices"]', 6, true, '2026-09-30 21:40:38.369372+05:30', NULL);
INSERT INTO public."Services" VALUES ('ba1f1a77-c1ad-4376-a26d-7e2834303c37', 'Custom Software Engineering', 'custom-software', 'Bespoke digital systems and automation engines tailored precisely to your operational workflow.', 'Off-the-shelf software rarely fits complex enterprise workflows. LilacTechSys engineers custom ERPs, supply chain automation tools, and internal management engines that eradicate operational bottlenecks and boost workforce throughput.', 'Code2', '["Enterprise ERP \u0026 CRM Tailoring","Legacy Modernization \u0026 Code Refactoring","Automated Data Pipelines \u0026 ETL Services","High-Throughput Distributed Microservices"]', '["Total ownership of software IP with zero licensing costs","Elimination of manual repetitive operational steps","Seamless integration with existing enterprise systems"]', '["C# / .NET 9","Python","PostgreSQL","Kafka","Docker","REST / gRPC"]', 3, true, '2026-09-30 21:40:38.369288+05:30', NULL);
INSERT INTO public."Services" VALUES ('ef2d25cf-37fc-447e-82ca-1ef492b511ac', 'Web Development', 'web-development', 'Scalable, high-performance web applications and enterprise portals built with modern frameworks.', 'From dynamic enterprise portals to high-concurrency SaaS applications, our engineering team constructs web platforms engineered for speed, high availability, and search visibility. We combine React, Next.js, ASP.NET Core, and PostgreSQL to deliver fluid, mission-critical digital experiences.', 'Globe', '["Custom SaaS Platforms \u0026 Web Portals","PWA (Progressive Web Apps) \u0026 Headless Commerce","SEO \u0026 Core Web Vitals Optimization","Micro-Frontends \u0026 Modular Component Libraries"]', '["Sub-second page load times for improved conversion","Enterprise-grade security and CSRF/XSS protection","High-availability architecture supporting 100k\u002B concurrent users"]', '["React.js","Vite","ASP.NET Core","Node.js","PostgreSQL","Tailwind CSS"]', 1, true, '2026-09-30 21:40:38.220031+05:30', NULL);
INSERT INTO public."Services" VALUES ('f704b6f4-6abf-41af-826f-d40c3be66c3f', 'UI/UX Design & Strategy', 'ui-ux-design', 'Human-centered digital product interfaces backed by user research, testing, and modern design systems.', 'Exceptional digital products balance aesthetic beauty with effortless usability. Our design studio conducts in-depth user journey mapping, rapid wireframing, and interactive prototyping to produce design systems that captivate users.', 'Palette', '["UX Research, Personas \u0026 User Journey Maps","Design Systems \u0026 Component Libraries (Figma)","High-Fidelity Interactive Prototypes","WCAG 2.1 AA Accessibility Audits \u0026 Remediation"]', '["Up to 3x increase in user task completion rates","Streamlined developer handoff reducing design debt","Consistent brand expression across web and mobile"]', '["Figma","Design Systems","Prototyping","UserTesting","Tailwind CSS"]', 5, true, '2026-09-30 21:40:38.369327+05:30', NULL);


--
-- Data for Name: Subscribers; Type: TABLE DATA; Schema: public; Owner: -
--



--
-- Data for Name: TeamMembers; Type: TABLE DATA; Schema: public; Owner: -
--

INSERT INTO public."TeamMembers" VALUES ('0b2367d5-fd4b-45c7-8122-75e7bfd50558', 'Meera Nair', 'Lead Full-Stack Solutions Engineer', 'Full-stack architect with deep expertise in React, ASP.NET Core, distributed PostgreSQL data modeling, and performance tuning.', 'https://images.unsplash.com/photo-1573497019940-1c28c88b4f3e?auto=format&fit=crop&w=400&q=80', 'Engineering', 'https://linkedin.com', NULL, 'https://github.com', 4, true, '2026-09-30 21:40:38.809421+05:30', NULL);
INSERT INTO public."TeamMembers" VALUES ('0e2da2c3-af08-458b-84ab-2f426d1bd73a', 'Dr. Arvind Patel', 'CEO & Chief Technology Strategist', 'Over 18 years driving enterprise digital transformation, cloud architecture, and mission-critical engineering solutions.', 'https://images.unsplash.com/photo-1534528741775-53994a69daeb?auto=format&fit=crop&w=400&q=80', 'Executive Leadership', 'https://linkedin.com', 'https://twitter.com', NULL, 1, true, '2026-09-30 21:40:38.808829+05:30', NULL);
INSERT INTO public."TeamMembers" VALUES ('5e74f7c9-8c43-4221-8327-31af3af934df', 'Liam O''Connor', 'Head of Cloud Architecture & Security', 'Cloud veteran specializing in Kubernetes, zero-trust infrastructure, automated CI/CD pipelines, and high-availability systems.', 'https://images.unsplash.com/photo-1519085360753-af0119f7cbe7?auto=format&fit=crop&w=400&q=80', 'Cloud & DevOps', 'https://linkedin.com', NULL, 'https://github.com', 3, true, '2026-09-30 21:40:38.809362+05:30', NULL);
INSERT INTO public."TeamMembers" VALUES ('e61bfb83-56e2-42ab-9afb-2dbc7da1e86a', 'Sophia Vance', 'Chief Design Officer', 'Award-winning UX strategist passionate about human-centered design systems, accessibility, and high-conversion user interfaces.', 'https://images.unsplash.com/photo-1580489944761-15a19d654956?auto=format&fit=crop&w=400&q=80', 'Product & Design', 'https://linkedin.com', NULL, NULL, 2, true, '2026-09-30 21:40:38.809361+05:30', NULL);


--
-- Data for Name: Testimonials; Type: TABLE DATA; Schema: public; Owner: -
--

INSERT INTO public."Testimonials" VALUES ('66e5a7d1-9bdf-4b5e-ae8a-793768c2cd2d', 'Marcus Chen', 'Founder & CEO', 'NexaPay Technologies', 'https://images.unsplash.com/photo-1500648767791-00dcc994a43e?auto=format&fit=crop&w=200&q=80', 5, 'LilacTechSys is the true definition of a high-tier strategic technology partner. They do not just write code—they think deeply about our business model, scalability vectors, and user retention.', 'Enterprise SaaS Overhaul', true, 4, '2026-09-30 21:40:38.742241+05:30', NULL);
INSERT INTO public."Testimonials" VALUES ('6861b62a-97d1-466b-ac7a-8b456c90ee69', 'Elena Rostova', 'Head of Global Logistics', 'OmniSupply International', 'https://images.unsplash.com/photo-1573496359142-b8d87734a5a2?auto=format&fit=crop&w=200&q=80', 5, 'The IoT telemetry mesh engineered by LilacTechSys gives our operations team real-time visibility across 35,000 shipments. Their DevOps automation and cloud scalability exceeded all our expectations.', 'OmniSupply IoT Platform', true, 3, '2026-09-30 21:40:38.742241+05:30', NULL);
INSERT INTO public."Testimonials" VALUES ('bbf958f2-61aa-4800-be8c-8cc98263dada', 'Sarah Jenkins', 'Chief Technology Officer', 'Aura Financial Group', 'https://images.unsplash.com/photo-1494790108377-be9c29b29330?auto=format&fit=crop&w=200&q=80', 5, 'LilacTechSys modernized our mission-critical settlement engine ahead of schedule. Their attention to security, high concurrency, and clean API design was world-class. Our payment latency dropped by over 80%.', 'AuraPay Global Platform', true, 1, '2026-09-30 21:40:38.741985+05:30', NULL);
INSERT INTO public."Testimonials" VALUES ('e320f66c-dfd2-4301-b430-2ada5e9f28c4', 'David Thorne', 'VP of Digital Engineering', 'Apex Health Network', 'https://images.unsplash.com/photo-1507003211169-0a1dd7228f2d?auto=format&fit=crop&w=200&q=80', 5, 'Building a HIPAA-compliant telemedicine platform is notoriously complex. LilacTechSys brought exceptional technical acumen and user experience finesse. Our doctors and patients love the new experience.', 'ApexHealth EHR Portal', true, 2, '2026-09-30 21:40:38.74224+05:30', NULL);


--
-- Name: AdminUsers PK_AdminUsers; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."AdminUsers"
    ADD CONSTRAINT "PK_AdminUsers" PRIMARY KEY ("Id");


--
-- Name: BlogPosts PK_BlogPosts; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."BlogPosts"
    ADD CONSTRAINT "PK_BlogPosts" PRIMARY KEY ("Id");


--
-- Name: Categories PK_Categories; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."Categories"
    ADD CONSTRAINT "PK_Categories" PRIMARY KEY ("Id");


--
-- Name: ContactMessages PK_ContactMessages; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."ContactMessages"
    ADD CONSTRAINT "PK_ContactMessages" PRIMARY KEY ("Id");


--
-- Name: JobApplications PK_JobApplications; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."JobApplications"
    ADD CONSTRAINT "PK_JobApplications" PRIMARY KEY ("Id");


--
-- Name: JobOpenings PK_JobOpenings; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."JobOpenings"
    ADD CONSTRAINT "PK_JobOpenings" PRIMARY KEY ("Id");


--
-- Name: Projects PK_Projects; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."Projects"
    ADD CONSTRAINT "PK_Projects" PRIMARY KEY ("Id");


--
-- Name: QuoteRequests PK_QuoteRequests; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."QuoteRequests"
    ADD CONSTRAINT "PK_QuoteRequests" PRIMARY KEY ("Id");


--
-- Name: Services PK_Services; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."Services"
    ADD CONSTRAINT "PK_Services" PRIMARY KEY ("Id");


--
-- Name: Subscribers PK_Subscribers; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."Subscribers"
    ADD CONSTRAINT "PK_Subscribers" PRIMARY KEY ("Id");


--
-- Name: TeamMembers PK_TeamMembers; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."TeamMembers"
    ADD CONSTRAINT "PK_TeamMembers" PRIMARY KEY ("Id");


--
-- Name: Testimonials PK_Testimonials; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."Testimonials"
    ADD CONSTRAINT "PK_Testimonials" PRIMARY KEY ("Id");


--
-- Name: IX_AdminUsers_Email; Type: INDEX; Schema: public; Owner: -
--

CREATE UNIQUE INDEX "IX_AdminUsers_Email" ON public."AdminUsers" USING btree ("Email");


--
-- Name: IX_AdminUsers_Username; Type: INDEX; Schema: public; Owner: -
--

CREATE UNIQUE INDEX "IX_AdminUsers_Username" ON public."AdminUsers" USING btree ("Username");


--
-- Name: IX_BlogPosts_CategoryId; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX "IX_BlogPosts_CategoryId" ON public."BlogPosts" USING btree ("CategoryId");


--
-- Name: IX_BlogPosts_Slug; Type: INDEX; Schema: public; Owner: -
--

CREATE UNIQUE INDEX "IX_BlogPosts_Slug" ON public."BlogPosts" USING btree ("Slug");


--
-- Name: IX_Categories_Slug; Type: INDEX; Schema: public; Owner: -
--

CREATE UNIQUE INDEX "IX_Categories_Slug" ON public."Categories" USING btree ("Slug");


--
-- Name: IX_JobApplications_JobOpeningId; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX "IX_JobApplications_JobOpeningId" ON public."JobApplications" USING btree ("JobOpeningId");


--
-- Name: IX_JobOpenings_Slug; Type: INDEX; Schema: public; Owner: -
--

CREATE UNIQUE INDEX "IX_JobOpenings_Slug" ON public."JobOpenings" USING btree ("Slug");


--
-- Name: IX_Projects_CategoryId; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX "IX_Projects_CategoryId" ON public."Projects" USING btree ("CategoryId");


--
-- Name: IX_Projects_Slug; Type: INDEX; Schema: public; Owner: -
--

CREATE UNIQUE INDEX "IX_Projects_Slug" ON public."Projects" USING btree ("Slug");


--
-- Name: IX_Services_Slug; Type: INDEX; Schema: public; Owner: -
--

CREATE UNIQUE INDEX "IX_Services_Slug" ON public."Services" USING btree ("Slug");


--
-- Name: IX_Subscribers_Email; Type: INDEX; Schema: public; Owner: -
--

CREATE UNIQUE INDEX "IX_Subscribers_Email" ON public."Subscribers" USING btree ("Email");


--
-- Name: BlogPosts FK_BlogPosts_Categories_CategoryId; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."BlogPosts"
    ADD CONSTRAINT "FK_BlogPosts_Categories_CategoryId" FOREIGN KEY ("CategoryId") REFERENCES public."Categories"("Id") ON DELETE RESTRICT;


--
-- Name: JobApplications FK_JobApplications_JobOpenings_JobOpeningId; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."JobApplications"
    ADD CONSTRAINT "FK_JobApplications_JobOpenings_JobOpeningId" FOREIGN KEY ("JobOpeningId") REFERENCES public."JobOpenings"("Id") ON DELETE CASCADE;


--
-- Name: Projects FK_Projects_Categories_CategoryId; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."Projects"
    ADD CONSTRAINT "FK_Projects_Categories_CategoryId" FOREIGN KEY ("CategoryId") REFERENCES public."Categories"("Id") ON DELETE RESTRICT;


--
-- PostgreSQL database dump complete
--

