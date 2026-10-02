using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using LilacTechSys.Domain.Entities;
using LilacTechSys.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace LilacTechSys.Infrastructure.Data
{
    public static class SeedData
    {
        public static async Task SeedAsync(LilacDbContext context)
        {
            // Seed Admin User
            if (!await context.AdminUsers.AnyAsync())
            {
                var admin = new AdminUser
                {
                    Id = Guid.NewGuid(),
                    Username = "admin",
                    Email = "admin@lilactechsys.com",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin@123"),
                    FullName = "Lilac Admin",
                    Role = UserRole.SuperAdmin,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                };
                context.AdminUsers.Add(admin);
            }

            // Seed Categories
            var categories = new List<Category>();
            if (!await context.Categories.AnyAsync())
            {
                categories = new List<Category>
                {
                    new Category
                    {
                        Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                        Name = "Cloud & DevOps",
                        Slug = "cloud-devops",
                        Description = "Enterprise cloud migration, Kubernetes orchestration, and continuous delivery.",
                        DisplayOrder = 1
                    },
                    new Category
                    {
                        Id = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                        Name = "Enterprise Web & SaaS",
                        Slug = "enterprise-web-saas",
                        Description = "Scalable web applications, customer portals, and mission-critical SaaS platforms.",
                        DisplayOrder = 2
                    },
                    new Category
                    {
                        Id = Guid.Parse("33333333-3333-3333-3333-333333333333"),
                        Name = "Mobile & Cross-Platform",
                        Slug = "mobile-apps",
                        Description = "High-performance iOS, Android, and cross-platform Flutter/React Native solutions.",
                        DisplayOrder = 3
                    },
                    new Category
                    {
                        Id = Guid.Parse("44444444-4444-4444-4444-444444444444"),
                        Name = "Cybersecurity & Governance",
                        Slug = "cybersecurity",
                        Description = "Zero-trust architecture, threat defense, compliance, and vulnerability assessments.",
                        DisplayOrder = 4
                    }
                };
                context.Categories.AddRange(categories);
                await context.SaveChangesAsync();
            }
            else
            {
                categories = await context.Categories.ToListAsync();
            }

            // Seed 8 Core Services
            if (!await context.Services.AnyAsync())
            {
                var services = new List<Service>
                {
                    new Service
                    {
                        Title = "Web Development",
                        Slug = "web-development",
                        ShortDescription = "Scalable, high-performance web applications and enterprise portals built with modern frameworks.",
                        DetailedDescription = "From dynamic enterprise portals to high-concurrency SaaS applications, our engineering team constructs web platforms engineered for speed, high availability, and search visibility. We combine React, Next.js, ASP.NET Core, and PostgreSQL to deliver fluid, mission-critical digital experiences.",
                        Icon = "Globe",
                        FeaturesJson = JsonSerializer.Serialize(new[]
                        {
                            "Custom SaaS Platforms & Web Portals",
                            "PWA (Progressive Web Apps) & Headless Commerce",
                            "SEO & Core Web Vitals Optimization",
                            "Micro-Frontends & Modular Component Libraries"
                        }),
                        BenefitsJson = JsonSerializer.Serialize(new[]
                        {
                            "Sub-second page load times for improved conversion",
                            "Enterprise-grade security and CSRF/XSS protection",
                            "High-availability architecture supporting 100k+ concurrent users"
                        }),
                        TechnologiesJson = JsonSerializer.Serialize(new[] { "React.js", "Vite", "ASP.NET Core", "Node.js", "PostgreSQL", "Tailwind CSS" }),
                        DisplayOrder = 1,
                        IsActive = true
                    },
                    new Service
                    {
                        Title = "Mobile App Development",
                        Slug = "mobile-app-development",
                        ShortDescription = "Fluid, native and cross-platform mobile apps for iOS and Android that elevate customer engagement.",
                        DetailedDescription = "Transform ideas into frictionless mobile touchpoints. We build consumer-facing and enterprise mobile applications utilizing React Native, Flutter, and native iOS/Android codebases that provide 60fps performance and secure offline capabilities.",
                        Icon = "Smartphone",
                        FeaturesJson = JsonSerializer.Serialize(new[]
                        {
                            "Cross-Platform iOS & Android Apps",
                            "Offline First Architecture & Data Sync",
                            "Biometric Authentication & Hardware Integration",
                            "App Store Optimization (ASO) & Deployment"
                        }),
                        BenefitsJson = JsonSerializer.Serialize(new[]
                        {
                            "Single codebase reducing engineering overhead by up to 40%",
                            "Consistent design system matching brand identity",
                            "Instant push notifications and real-time interaction"
                        }),
                        TechnologiesJson = JsonSerializer.Serialize(new[] { "React Native", "Flutter", "Swift", "Kotlin", "Firebase", "WebSockets" }),
                        DisplayOrder = 2,
                        IsActive = true
                    },
                    new Service
                    {
                        Title = "Custom Software Engineering",
                        Slug = "custom-software",
                        ShortDescription = "Bespoke digital systems and automation engines tailored precisely to your operational workflow.",
                        DetailedDescription = "Off-the-shelf software rarely fits complex enterprise workflows. LilacTechSys engineers custom ERPs, supply chain automation tools, and internal management engines that eradicate operational bottlenecks and boost workforce throughput.",
                        Icon = "Code2",
                        FeaturesJson = JsonSerializer.Serialize(new[]
                        {
                            "Enterprise ERP & CRM Tailoring",
                            "Legacy Modernization & Code Refactoring",
                            "Automated Data Pipelines & ETL Services",
                            "High-Throughput Distributed Microservices"
                        }),
                        BenefitsJson = JsonSerializer.Serialize(new[]
                        {
                            "Total ownership of software IP with zero licensing costs",
                            "Elimination of manual repetitive operational steps",
                            "Seamless integration with existing enterprise systems"
                        }),
                        TechnologiesJson = JsonSerializer.Serialize(new[] { "C# / .NET 9", "Python", "PostgreSQL", "Kafka", "Docker", "REST / gRPC" }),
                        DisplayOrder = 3,
                        IsActive = true
                    },
                    new Service
                    {
                        Title = "Cloud & DevOps",
                        Slug = "cloud-and-devops",
                        ShortDescription = "Automated CI/CD pipelines, Kubernetes container orchestration, and multi-cloud resilience.",
                        DetailedDescription = "Accelerate delivery cadences with enterprise DevOps. We engineer immutable infrastructure-as-code, self-healing Kubernetes clusters, and zero-downtime deployment pipelines across AWS, Azure, and Google Cloud.",
                        Icon = "Cloud",
                        FeaturesJson = JsonSerializer.Serialize(new[]
                        {
                            "Infrastructure as Code (Terraform, Pulumi)",
                            "Kubernetes Cluster Deployment & GitOps (ArgoCD)",
                            "Automated CI/CD Pipelines (GitHub Actions, GitLab)",
                            "Multi-Region Failover & Disaster Recovery"
                        }),
                        BenefitsJson = JsonSerializer.Serialize(new[]
                        {
                            "Daily release cadence with automated quality gates",
                            "99.99% infrastructure uptime with auto-scaling",
                            "30%+ reduction in unnecessary cloud resource spend"
                        }),
                        TechnologiesJson = JsonSerializer.Serialize(new[] { "AWS", "Azure", "Docker", "Kubernetes", "Terraform", "GitHub Actions" }),
                        DisplayOrder = 4,
                        IsActive = true
                    },
                    new Service
                    {
                        Title = "UI/UX Design & Strategy",
                        Slug = "ui-ux-design",
                        ShortDescription = "Human-centered digital product interfaces backed by user research, testing, and modern design systems.",
                        DetailedDescription = "Exceptional digital products balance aesthetic beauty with effortless usability. Our design studio conducts in-depth user journey mapping, rapid wireframing, and interactive prototyping to produce design systems that captivate users.",
                        Icon = "Palette",
                        FeaturesJson = JsonSerializer.Serialize(new[]
                        {
                            "UX Research, Personas & User Journey Maps",
                            "Design Systems & Component Libraries (Figma)",
                            "High-Fidelity Interactive Prototypes",
                            "WCAG 2.1 AA Accessibility Audits & Remediation"
                        }),
                        BenefitsJson = JsonSerializer.Serialize(new[]
                        {
                            "Up to 3x increase in user task completion rates",
                            "Streamlined developer handoff reducing design debt",
                            "Consistent brand expression across web and mobile"
                        }),
                        TechnologiesJson = JsonSerializer.Serialize(new[] { "Figma", "Design Systems", "Prototyping", "UserTesting", "Tailwind CSS" }),
                        DisplayOrder = 5,
                        IsActive = true
                    },
                    new Service
                    {
                        Title = "IT Consulting & Advisory",
                        Slug = "it-consulting",
                        ShortDescription = "Strategic technology roadmapping, enterprise architecture design, and CTO-level guidance.",
                        DetailedDescription = "Navigate complex technology investments with clarity. Our seasoned enterprise architects audit existing stacks, identify technical debt, and formulate phased modernization roadmaps aligned directly with revenue goals.",
                        Icon = "Briefcase",
                        FeaturesJson = JsonSerializer.Serialize(new[]
                        {
                            "Digital Transformation Roadmaps",
                            "Enterprise Architecture & Stack Audits",
                            "Cloud Migration Planning & TCO Analysis",
                            "Fractional CTO & Technical Due Diligence"
                        }),
                        BenefitsJson = JsonSerializer.Serialize(new[]
                        {
                            "De-risked technology modernization initiatives",
                            "Direct alignment between IT expenditure and business value",
                            "Objective vendor evaluation free of commercial bias"
                        }),
                        TechnologiesJson = JsonSerializer.Serialize(new[] { "Enterprise Architecture", "TOGAF", "Cloud Economics", "Microservices" }),
                        DisplayOrder = 6,
                        IsActive = true
                    },
                    new Service
                    {
                        Title = "Cybersecurity & Compliance",
                        Slug = "cybersecurity",
                        ShortDescription = "Zero-trust defenses, continuous vulnerability monitoring, penetration testing, and compliance readiness.",
                        DetailedDescription = "Safeguard intellectual property and sensitive customer data. We implement zero-trust network architectures, perform vulnerability assessments, and ensure compliance readiness across ISO 27001, SOC 2, and GDPR.",
                        Icon = "ShieldCheck",
                        FeaturesJson = JsonSerializer.Serialize(new[]
                        {
                            "Zero-Trust Architecture & IAM Governance",
                            "Automated Vulnerability Scanning & Pentesting",
                            "SOC 2, ISO 27001 & GDPR Compliance Frameworks",
                            "Incident Response Strategy & Threat Modeling"
                        }),
                        BenefitsJson = JsonSerializer.Serialize(new[]
                        {
                            "Proactive mitigation of breach and ransom vulnerabilities",
                            "Frictionless customer trust and audit readiness",
                            "24/7 endpoint visibility and rapid threat containment"
                        }),
                        TechnologiesJson = JsonSerializer.Serialize(new[] { "Zero-Trust", "OAuth2 / OIDC", "WAF", "SonarQube", "OWASP Top 10" }),
                        DisplayOrder = 7,
                        IsActive = true
                    },
                    new Service
                    {
                        Title = "Maintenance & 24/7 Support",
                        Slug = "maintenance-and-support",
                        ShortDescription = "Dedicated SLAs, preventative monitoring, security patching, and ongoing performance tuning.",
                        DetailedDescription = "Keep your software reliable, fast, and secure post-launch. Our dedicated site reliability engineers provide SLA-backed monitoring, bug fixes, routine dependency upgrades, and ongoing cloud cost optimizations.",
                        Icon = "Headphones",
                        FeaturesJson = JsonSerializer.Serialize(new[]
                        {
                            "24/7/365 Incident Response & Uptime SLAs",
                            "Scheduled Security Patching & Library Upgrades",
                            "Real-Time APM Monitoring & Log Diagnostics",
                            "Continuous Performance & Database Tuning"
                        }),
                        BenefitsJson = JsonSerializer.Serialize(new[]
                        {
                            "Guaranteed 15-minute response SLA for critical incidents",
                            "Elimination of unexpected software decay and bugs",
                            "Predictable monthly operational budgets"
                        }),
                        TechnologiesJson = JsonSerializer.Serialize(new[] { "Datadog", "Prometheus", "Grafana", "Sentry", "PagerDuty" }),
                        DisplayOrder = 8,
                        IsActive = true
                    }
                };
                context.Services.AddRange(services);
                await context.SaveChangesAsync();
            }

            // Seed Projects
            if (!await context.Projects.AnyAsync())
            {
                var webCatId = categories.First(c => c.Slug == "enterprise-web-saas").Id;
                var cloudCatId = categories.First(c => c.Slug == "cloud-devops").Id;
                var mobileCatId = categories.First(c => c.Slug == "mobile-apps").Id;
                var secCatId = categories.First(c => c.Slug == "cybersecurity").Id;

                var projects = new List<Project>
                {
                    new Project
                    {
                        Title = "AuraPay Global Banking & Settlement Platform",
                        Slug = "aurapay-global-platform",
                        ClientName = "Aura Financial Group",
                        Summary = "Modernized cross-border payments infrastructure processing over $40M daily with sub-second clearing.",
                        FullDescription = "Aura Financial Group faced severe concurrency bottlenecks on legacy payment gateways. LilacTechSys engineered a modular cloud-native settlement platform incorporating event-driven microservices and localized payment adapters.",
                        Challenge = "Legacy monolithic architecture suffered from 4.2-second average transaction latency and intermittent timeouts during peak European settlement hours.",
                        Solution = "Architected an event-driven ASP.NET Core 9 and PostgreSQL microservice cluster with distributed Redis caching, Kafka transaction queues, and a responsive React management portal.",
                        ResultsJson = JsonSerializer.Serialize(new[]
                        {
                            new { metric = "82%", label = "Reduction in Latency" },
                            new { metric = "99.995%", label = "System Uptime" },
                            new { metric = "$40M+", label = "Daily Settlement Volume" }
                        }),
                        ThumbnailUrl = "https://images.unsplash.com/photo-1559526324-4b87b5e36e44?auto=format&fit=crop&w=800&q=80",
                        BannerUrl = "https://images.unsplash.com/photo-1559526324-4b87b5e36e44?auto=format&fit=crop&w=1600&q=80",
                        GalleryJson = JsonSerializer.Serialize(new[]
                        {
                            "https://images.unsplash.com/photo-1559526324-4b87b5e36e44?auto=format&fit=crop&w=800&q=80",
                            "https://images.unsplash.com/photo-1551288049-bebda4e38f71?auto=format&fit=crop&w=800&q=80"
                        }),
                        TechStackJson = JsonSerializer.Serialize(new[] { "React.js", "ASP.NET Core", "PostgreSQL", "Kafka", "Docker", "Tailwind CSS" }),
                        ProjectUrl = "https://aurapay.example.com",
                        CategoryId = webCatId,
                        IsFeatured = true,
                        DisplayOrder = 1
                    },
                    new Project
                    {
                        Title = "ApexHealth EHR & Telemedicine Engine",
                        Slug = "apexhealth-ehr-telemedicine",
                        ClientName = "Apex Health Network",
                        Summary = "HIPAA-compliant telemedicine engine connecting 120,000+ patients with certified healthcare providers.",
                        FullDescription = "Apex Health Network required a seamless, high-security telemedicine portal with encrypted WebRTC video visits, automated medical transcription, and real-time electronic health records synchronization.",
                        Challenge = "Fragmented EHR systems caused double-entry for doctors and unacceptable appointment drop-offs due to video codec incompatibilities on mobile browsers.",
                        Solution = "Built a unified WebRTC-powered portal with end-to-end encrypted medical data pipelines and automated physician workflow tools built with React, ASP.NET Core, and WebSockets.",
                        ResultsJson = JsonSerializer.Serialize(new[]
                        {
                            new { metric = "120K+", label = "Active Patients" },
                            new { metric = "4.9/5", label = "Provider Rating" },
                            new { metric = "100%", label = "HIPAA Compliance" }
                        }),
                        ThumbnailUrl = "https://images.unsplash.com/photo-1576091160399-112ba8d25d1d?auto=format&fit=crop&w=800&q=80",
                        BannerUrl = "https://images.unsplash.com/photo-1576091160399-112ba8d25d1d?auto=format&fit=crop&w=1600&q=80",
                        GalleryJson = JsonSerializer.Serialize(new[]
                        {
                            "https://images.unsplash.com/photo-1576091160399-112ba8d25d1d?auto=format&fit=crop&w=800&q=80"
                        }),
                        TechStackJson = JsonSerializer.Serialize(new[] { "React.js", "WebRTC", ".NET 9", "PostgreSQL", "Tailwind CSS" }),
                        ProjectUrl = "https://apexhealth.example.com",
                        CategoryId = mobileCatId,
                        IsFeatured = true,
                        DisplayOrder = 2
                    },
                    new Project
                    {
                        Title = "OmniSupply Real-Time IoT Logistics Mesh",
                        Slug = "omnisupply-iot-logistics",
                        ClientName = "OmniSupply Global",
                        Summary = "Fleet telemetry and warehouse automation tracking over 35,000 cross-continental shipments in real time.",
                        FullDescription = "OmniSupply needed real-time visibility into temperature-sensitive pharmaceutical shipments with predictive delivery alerts and automated customs clearance documentation.",
                        Challenge = "Unreliable cellular coverage during transit resulted in lost telemetry and missed temperature spike alerts for vaccine cargo.",
                        Solution = "Designed an edge-computing gateway with store-and-forward telemetry, cloud event streaming, and an intuitive live operations map built with React and Mapbox.",
                        ResultsJson = JsonSerializer.Serialize(new[]
                        {
                            new { metric = "35K+", label = "Tracked Cargo Units" },
                            new { metric = "-34%", label = "Spoilage Incidents" },
                            new { metric = "99.8%", label = "Telemetry Accuracy" }
                        }),
                        ThumbnailUrl = "https://images.unsplash.com/photo-1586528116311-ad8dd3c8310d?auto=format&fit=crop&w=800&q=80",
                        BannerUrl = "https://images.unsplash.com/photo-1586528116311-ad8dd3c8310d?auto=format&fit=crop&w=1600&q=80",
                        GalleryJson = JsonSerializer.Serialize(new[]
                        {
                            "https://images.unsplash.com/photo-1586528116311-ad8dd3c8310d?auto=format&fit=crop&w=800&q=80"
                        }),
                        TechStackJson = JsonSerializer.Serialize(new[] { "React", "TypeScript", "ASP.NET Core", "TimescaleDB", "Kubernetes" }),
                        CategoryId = cloudCatId,
                        IsFeatured = true,
                        DisplayOrder = 3
                    },
                    new Project
                    {
                        Title = "ZeroGuard Enterprise Access & Security Mesh",
                        Slug = "zeroguard-security-mesh",
                        ClientName = "DefenSys Corporation",
                        Summary = "Implemented a zero-trust software-defined perimeter protecting 8,000 corporate devices from insider and external threats.",
                        FullDescription = "DefenSys required a complete migration from brittle corporate VPNs to an identity-aware proxy architecture with granular conditional access and continuous posture verification.",
                        Challenge = "Remote work expansion overloaded legacy VPN concentrators, resulting in security blind spots and cumbersome user access friction.",
                        Solution = "Implemented an OAuth2/OIDC identity proxy with device compliance posture checking, automated revocation triggers, and a unified employee self-service portal.",
                        ResultsJson = JsonSerializer.Serialize(new[]
                        {
                            new { metric = "0", label = "Perimeter Breaches" },
                            new { metric = "70%", label = "Drop in Helpdesk Tickets" },
                            new { metric = "8,000", label = "Secured Endpoints" }
                        }),
                        ThumbnailUrl = "https://images.unsplash.com/photo-1563986768609-322da13575f3?auto=format&fit=crop&w=800&q=80",
                        BannerUrl = "https://images.unsplash.com/photo-1563986768609-322da13575f3?auto=format&fit=crop&w=1600&q=80",
                        GalleryJson = JsonSerializer.Serialize(new[]
                        {
                            "https://images.unsplash.com/photo-1563986768609-322da13575f3?auto=format&fit=crop&w=800&q=80"
                        }),
                        TechStackJson = JsonSerializer.Serialize(new[] { "Zero-Trust", "ASP.NET Core", "PostgreSQL", "React", "Docker" }),
                        CategoryId = secCatId,
                        IsFeatured = true,
                        DisplayOrder = 4
                    }
                };
                context.Projects.AddRange(projects);
                await context.SaveChangesAsync();
            }

            // Seed Blog Posts
            if (!await context.BlogPosts.AnyAsync())
            {
                var cloudCatId = categories.First(c => c.Slug == "cloud-devops").Id;
                var secCatId = categories.First(c => c.Slug == "cybersecurity").Id;
                var webCatId = categories.First(c => c.Slug == "enterprise-web-saas").Id;

                var blogs = new List<BlogPost>
                {
                    new BlogPost
                    {
                        Title = "Architecting Resilient Cloud-Native Microservices in 2026",
                        Slug = "architecting-resilient-cloud-native-microservices",
                        Excerpt = "How distributed caching, circuit breakers, and container orchestration ensure fault-tolerant systems under enterprise load.",
                        ContentHtml = @"<p>In modern enterprise engineering, resilience is not an accidental feature—it is an architectural discipline. Monolithic failures cascaded unchecked; distributed systems must isolate faults gracefully.</p>
<h3>1. The Circuit Breaker Pattern</h3>
<p>When an upstream dependency suffers elevated latency or errors, cascading pool exhaustion can bring down unrelated sub-systems. Utilizing Polly in .NET Core or Resilience4j creates adaptive circuit breakers that trip into fallback responses within milliseconds.</p>
<h3>2. Eventual Consistency with Outbox Patterns</h3>
<p>Distributed transactions across microservices introduce brittle two-phase locks. Implementing the Transactional Outbox Pattern ensures state changes and Kafka events are committed atomically inside your local PostgreSQL database, eliminating message loss.</p>
<h3>3. Observability Over Mere Monitoring</h3>
<p>Modern Kubernetes deployments require OpenTelemetry distributed tracing to map request spans end-to-end across multiple container boundaries.</p>",
                        CoverImageUrl = "https://images.unsplash.com/photo-1451187580459-43490279c0fa?auto=format&fit=crop&w=800&q=80",
                        AuthorName = "Liam O'Connor",
                        AuthorRole = "Head of Cloud Architecture",
                        CategoryId = cloudCatId,
                        TagsJson = JsonSerializer.Serialize(new[] { "Microservices", "Cloud", "Kubernetes", "Architecture" }),
                        ReadTimeMinutes = 6,
                        ViewCount = 428,
                        IsPublished = true,
                        PublishedAt = DateTime.UtcNow.AddDays(-14)
                    },
                    new BlogPost
                    {
                        Title = "The Zero-Trust Security Paradigm: Practical Enterprise Guide",
                        Slug = "zero-trust-security-paradigm-guide",
                        Excerpt = "Moving past legacy VPNs into continuous identity verification, device attestation, and least-privilege access models.",
                        ContentHtml = @"<p>The traditional castle-and-moat security perimeter is obsolete. Modern workforces are hybrid, workloads live in multi-cloud clusters, and attacks originate from compromised user credentials.</p>
<h3>Never Trust, Always Verify</h3>
<p>Zero Trust enforces strict identity validation for every person and device attempting to access network resources, regardless of whether they sit inside or outside the corporate intranet.</p>
<h3>Core Tenets of Zero Trust</h3>
<ul>
<li><strong>Explicit Identity Verification:</strong> Leverage multi-factor authentication (MFA) backed by FIDO2 keys.</li>
<li><strong>Least Privilege Access:</strong> Just-in-time access tokens with strict time-to-live boundaries.</li>
<li><strong>Assume Breach:</strong> Segment network micro-perimeters so lateral movement by bad actors is blocked.</li>
</ul>",
                        CoverImageUrl = "https://images.unsplash.com/photo-1550751827-4bd374c3f58b?auto=format&fit=crop&w=800&q=80",
                        AuthorName = "Dr. Arvind Patel",
                        AuthorRole = "CEO & Tech Strategist",
                        CategoryId = secCatId,
                        TagsJson = JsonSerializer.Serialize(new[] { "Cybersecurity", "Zero-Trust", "Compliance", "Security" }),
                        ReadTimeMinutes = 8,
                        ViewCount = 612,
                        IsPublished = true,
                        PublishedAt = DateTime.UtcNow.AddDays(-7)
                    },
                    new BlogPost
                    {
                        Title = "Why Modern Enterprises Are Shifting to Headless Web Architecture",
                        Slug = "why-enterprises-shift-to-headless-web",
                        Excerpt = "Separating frontend presentation from backend logic delivers faster speed, superior omnichannel reach, and enhanced security.",
                        ContentHtml = @"<p>Coupled monolithic CMS platforms once ruled the web. Today, omnichannel demands—from mobile apps to smart kiosks and lightning-fast web storefronts—demand API-first decoupled architectures.</p>
<h3>Benefits of Headless Systems</h3>
<p>By consuming structured REST or GraphQL APIs, frontend engineers can leverage modern component frameworks like React and Vite to deliver instantaneous sub-second user interactions without waiting on legacy server render cycles.</p>",
                        CoverImageUrl = "https://images.unsplash.com/photo-1460925895917-afdab827c52f?auto=format&fit=crop&w=800&q=80",
                        AuthorName = "Sophia Vance",
                        AuthorRole = "Chief Design Officer",
                        CategoryId = webCatId,
                        TagsJson = JsonSerializer.Serialize(new[] { "Web Dev", "Headless", "React", "Performance" }),
                        ReadTimeMinutes = 5,
                        ViewCount = 385,
                        IsPublished = true,
                        PublishedAt = DateTime.UtcNow.AddDays(-2)
                    }
                };
                context.BlogPosts.AddRange(blogs);
                await context.SaveChangesAsync();
            }

            // Seed Testimonials
            if (!await context.Testimonials.AnyAsync())
            {
                var testimonials = new List<Testimonial>
                {
                    new Testimonial
                    {
                        ClientName = "Sarah Jenkins",
                        ClientTitle = "Chief Technology Officer",
                        CompanyName = "Aura Financial Group",
                        AvatarUrl = "https://images.unsplash.com/photo-1494790108377-be9c29b29330?auto=format&fit=crop&w=200&q=80",
                        Rating = 5,
                        Content = "LilacTechSys modernized our mission-critical settlement engine ahead of schedule. Their attention to security, high concurrency, and clean API design was world-class. Our payment latency dropped by over 80%.",
                        ProjectName = "AuraPay Global Platform",
                        IsFeatured = true,
                        DisplayOrder = 1
                    },
                    new Testimonial
                    {
                        ClientName = "David Thorne",
                        ClientTitle = "VP of Digital Engineering",
                        CompanyName = "Apex Health Network",
                        AvatarUrl = "https://images.unsplash.com/photo-1507003211169-0a1dd7228f2d?auto=format&fit=crop&w=200&q=80",
                        Rating = 5,
                        Content = "Building a HIPAA-compliant telemedicine platform is notoriously complex. LilacTechSys brought exceptional technical acumen and user experience finesse. Our doctors and patients love the new experience.",
                        ProjectName = "ApexHealth EHR Portal",
                        IsFeatured = true,
                        DisplayOrder = 2
                    },
                    new Testimonial
                    {
                        ClientName = "Elena Rostova",
                        ClientTitle = "Head of Global Logistics",
                        CompanyName = "OmniSupply International",
                        AvatarUrl = "https://images.unsplash.com/photo-1573496359142-b8d87734a5a2?auto=format&fit=crop&w=200&q=80",
                        Rating = 5,
                        Content = "The IoT telemetry mesh engineered by LilacTechSys gives our operations team real-time visibility across 35,000 shipments. Their DevOps automation and cloud scalability exceeded all our expectations.",
                        ProjectName = "OmniSupply IoT Platform",
                        IsFeatured = true,
                        DisplayOrder = 3
                    },
                    new Testimonial
                    {
                        ClientName = "Marcus Chen",
                        ClientTitle = "Founder & CEO",
                        CompanyName = "NexaPay Technologies",
                        AvatarUrl = "https://images.unsplash.com/photo-1500648767791-00dcc994a43e?auto=format&fit=crop&w=200&q=80",
                        Rating = 5,
                        Content = "LilacTechSys is the true definition of a high-tier strategic technology partner. They do not just write code—they think deeply about our business model, scalability vectors, and user retention.",
                        ProjectName = "Enterprise SaaS Overhaul",
                        IsFeatured = true,
                        DisplayOrder = 4
                    }
                };
                context.Testimonials.AddRange(testimonials);
                await context.SaveChangesAsync();
            }

            // Seed Team Members
            if (!await context.TeamMembers.AnyAsync())
            {
                var team = new List<TeamMember>
                {
                    new TeamMember
                    {
                        FullName = "Dr. Arvind Patel",
                        Role = "CEO & Chief Technology Strategist",
                        Bio = "Over 18 years driving enterprise digital transformation, cloud architecture, and mission-critical engineering solutions.",
                        AvatarUrl = "https://images.unsplash.com/photo-1534528741775-53994a69daeb?auto=format&fit=crop&w=400&q=80",
                        Department = "Executive Leadership",
                        LinkedInUrl = "https://linkedin.com",
                        TwitterUrl = "https://twitter.com",
                        DisplayOrder = 1,
                        IsActive = true
                    },
                    new TeamMember
                    {
                        FullName = "Sophia Vance",
                        Role = "Chief Design Officer",
                        Bio = "Award-winning UX strategist passionate about human-centered design systems, accessibility, and high-conversion user interfaces.",
                        AvatarUrl = "https://images.unsplash.com/photo-1580489944761-15a19d654956?auto=format&fit=crop&w=400&q=80",
                        Department = "Product & Design",
                        LinkedInUrl = "https://linkedin.com",
                        DisplayOrder = 2,
                        IsActive = true
                    },
                    new TeamMember
                    {
                        FullName = "Liam O'Connor",
                        Role = "Head of Cloud Architecture & Security",
                        Bio = "Cloud veteran specializing in Kubernetes, zero-trust infrastructure, automated CI/CD pipelines, and high-availability systems.",
                        AvatarUrl = "https://images.unsplash.com/photo-1519085360753-af0119f7cbe7?auto=format&fit=crop&w=400&q=80",
                        Department = "Cloud & DevOps",
                        LinkedInUrl = "https://linkedin.com",
                        GithubUrl = "https://github.com",
                        DisplayOrder = 3,
                        IsActive = true
                    },
                    new TeamMember
                    {
                        FullName = "Meera Nair",
                        Role = "Lead Full-Stack Solutions Engineer",
                        Bio = "Full-stack architect with deep expertise in React, ASP.NET Core, distributed PostgreSQL data modeling, and performance tuning.",
                        AvatarUrl = "https://images.unsplash.com/photo-1573497019940-1c28c88b4f3e?auto=format&fit=crop&w=400&q=80",
                        Department = "Engineering",
                        LinkedInUrl = "https://linkedin.com",
                        GithubUrl = "https://github.com",
                        DisplayOrder = 4,
                        IsActive = true
                    }
                };
                context.TeamMembers.AddRange(team);
                await context.SaveChangesAsync();
            }

            // Seed Careers / Job Openings
            if (!await context.JobOpenings.AnyAsync())
            {
                var jobs = new List<JobOpening>
                {
                    new JobOpening
                    {
                        Title = "Senior Full-Stack .NET & React Engineer",
                        Slug = "senior-full-stack-dotnet-react-engineer",
                        Department = "Engineering",
                        Location = "Remote (Global)",
                        Type = "Full-time",
                        ExperienceLevel = "Senior (5+ years)",
                        Description = "We are seeking an experienced full-stack engineer to lead the development of enterprise cloud applications using ASP.NET Core (.NET 9) and React.js.",
                        RequirementsJson = JsonSerializer.Serialize(new[]
                        {
                            "5+ years professional experience building web applications in C# / .NET Core and React",
                            "Deep understanding of relational databases (PostgreSQL/SQL Server) and query optimization",
                            "Familiarity with containerized environments (Docker, Kubernetes) and CI/CD pipelines",
                            "Strong communication skills and passion for clean, readable code and unit testing"
                        }),
                        ResponsibilitiesJson = JsonSerializer.Serialize(new[]
                        {
                            "Architect scalable backend APIs and high-performance frontend interfaces",
                            "Lead code reviews, design docs, and architectural alignment sessions",
                            "Collaborate closely with UI/UX designers and product managers"
                        }),
                        BenefitsJson = JsonSerializer.Serialize(new[]
                        {
                            "Competitive compensation with annual performance bonus",
                            "Flexible 100% remote working culture",
                            "$2,500 annual personal learning & tech conference stipend",
                            "Comprehensive health and wellness coverage"
                        }),
                        IsActive = true,
                        Deadline = DateTime.UtcNow.AddDays(45)
                    },
                    new JobOpening
                    {
                        Title = "Cloud & DevOps Solutions Architect",
                        Slug = "cloud-devops-solutions-architect",
                        Department = "Cloud & Security",
                        Location = "Hybrid / Remote",
                        Type = "Full-time",
                        ExperienceLevel = "Lead (7+ years)",
                        Description = "Lead the design and implementation of automated, self-healing cloud infrastructure and zero-downtime deployment pipelines.",
                        RequirementsJson = JsonSerializer.Serialize(new[]
                        {
                            "Extensive experience with AWS or Azure cloud architectures and Kubernetes",
                            "Proficiency in Terraform, Helm, and GitOps workflows (ArgoCD)",
                            "Deep knowledge of security standards (SOC 2, ISO 27001, Zero-Trust)"
                        }),
                        ResponsibilitiesJson = JsonSerializer.Serialize(new[]
                        {
                            "Define cloud architecture standards across all client engagements",
                            "Automate multi-region failover and infrastructure monitoring",
                            "Conduct architecture reviews and optimize cloud infrastructure costs"
                        }),
                        BenefitsJson = JsonSerializer.Serialize(new[]
                        {
                            "Executive level compensation package",
                            "Comprehensive health, vision, and dental insurance",
                            "Unlimited PTO policy with mandatory minimum rest days"
                        }),
                        IsActive = true,
                        Deadline = DateTime.UtcNow.AddDays(30)
                    },
                    new JobOpening
                    {
                        Title = "Lead UI/UX Product Designer",
                        Slug = "lead-ui-ux-product-designer",
                        Department = "Product & Design",
                        Location = "Remote",
                        Type = "Full-time",
                        ExperienceLevel = "Senior (4+ years)",
                        Description = "Craft world-class user interfaces and interactive experiences for cutting-edge SaaS, fintech, and healthcare solutions.",
                        RequirementsJson = JsonSerializer.Serialize(new[]
                        {
                            "Portfolio demonstrating high aesthetic polish, typography mastery, and responsive design",
                            "Expertise in Figma design systems, tokens, and auto-layout",
                            "Knowledge of WCAG accessibility standards and micro-interaction design"
                        }),
                        ResponsibilitiesJson = JsonSerializer.Serialize(new[]
                        {
                            "Own end-to-end design from user flows and wireframes to pixel-perfect design systems",
                            "Collaborate with frontend developers on animation and interaction fidelity",
                            "Facilitate client design workshops and prototype evaluations"
                        }),
                        BenefitsJson = JsonSerializer.Serialize(new[]
                        {
                            "Generous home office stipend for top-tier hardware and displays",
                            "Collaborative, design-first engineering environment",
                            "Annual wellness and fitness allowance"
                        }),
                        IsActive = true,
                        Deadline = DateTime.UtcNow.AddDays(60)
                    }
                };
                context.JobOpenings.AddRange(jobs);
                await context.SaveChangesAsync();
            }
        }
    }
}
