$endpoints = @(
    '/api/v1/health',
    '/api/v1/services',
    '/api/v1/projects',
    '/api/v1/blog',
    '/api/v1/careers',
    '/api/v1/testimonials',
    '/api/v1/team'
)

Write-Host "=== LILACTECHSYS API INTEGRATION CHECK ==="
foreach ($ep in $endpoints) {
    try {
        $res = Invoke-RestMethod -Uri ("http://localhost:5000" + $ep) -Method GET
        Write-Host "[OK] $ep -> Success: $($res.success), Count: $(if ($res.data -is [array]) { $res.data.Count } elseif ($res.data.items) { $res.data.items.Count } else { 'Object' })"
    } catch {
        Write-Host "[FAIL] $ep -> $_"
    }
}

# Test Authentication & JWT Login
Write-Host "`n=== TESTING AUTHENTICATION ==="
try {
    $loginBody = @{
        username = "admin"
        password = "Admin@123"
    } | ConvertTo-Json

    $loginRes = Invoke-RestMethod -Uri "http://localhost:5000/api/v1/auth/login" -Method POST -Body $loginBody -ContentType "application/json"
    Write-Host "[OK] /api/v1/auth/login -> Success: $($loginRes.success), User: $($loginRes.data.username), Role: $($loginRes.data.role), TokenLength: $($loginRes.data.accessToken.Length)"

    # Test Dashboard with Token
    $headers = @{
        Authorization = "Bearer $($loginRes.data.accessToken)"
    }
    $dashRes = Invoke-RestMethod -Uri "http://localhost:5000/api/v1/dashboard/stats" -Method GET -Headers $headers
    Write-Host "[OK] /api/v1/dashboard/stats -> Total Services: $($dashRes.data.totalServices), Total Projects: $($dashRes.data.totalProjects), Total Blogs: $($dashRes.data.totalBlogs), Total Inquiries: $($dashRes.data.totalInquiries)"
} catch {
    Write-Host "[FAIL] Auth Test -> $_"
}

# Test Public Quote Submission
Write-Host "`n=== TESTING QUOTE SUBMISSION ==="
try {
    $quoteBody = @{
        fullName = "Test Client"
        email = "test@company.com"
        phone = "+1234567890"
        companyName = "Global Ventures Inc"
        servicesNeeded = "Cloud Architecture & DevOps"
        estimatedBudget = "$25,000 - $50,000"
        targetTimeline = "1 - 3 Months"
        projectScope = "Automated CI/CD and multi-region Kubernetes migration"
    } | ConvertTo-Json

    $quoteRes = Invoke-RestMethod -Uri "http://localhost:5000/api/v1/quote" -Method POST -Body $quoteBody -ContentType "application/json"
    Write-Host "[OK] /api/v1/quote -> Success: $($quoteRes.success), Message: $($quoteRes.message)"
} catch {
    Write-Host "[FAIL] Quote Test -> $_"
}

# Test Public Contact Submission
Write-Host "`n=== TESTING CONTACT SUBMISSION ==="
try {
    $contactBody = @{
        fullName = "Jane Doe"
        email = "jane.doe@enterprise.org"
        phone = "+1987654321"
        subject = "Enterprise Security Audit"
        message = "We need an architectural review of our payment gateway."
    } | ConvertTo-Json

    $contactRes = Invoke-RestMethod -Uri "http://localhost:5000/api/v1/contact" -Method POST -Body $contactBody -ContentType "application/json"
    Write-Host "[OK] /api/v1/contact -> Success: $($contactRes.success), Message: $($contactRes.message)"
} catch {
    Write-Host "[FAIL] Contact Test -> $_"
}
