# 🚀 CI/CD Setup Instructions

## ⚡ Quick Start (5 Minutes)

### **Step 1: Commit and Push the Workflow**
```bash
cd "C:\Users\USER\source\repos\Buy&Sell"

# Verify git is configured
git status

# Add the new workflow file
git add .github/workflows/ci-cd.yml

# Commit the changes
git commit -m "feat: add GitHub Actions CI/CD pipeline with 87 tests"

# Push to GitHub
git push origin master
```

### **Step 2: Monitor Workflow on GitHub**
1. Go to: **https://github.com/Callmesammy/Buy-Sell/actions**
2. Click the newest workflow run
3. Watch tests execute in real-time
4. Check for ✅ (success) or ❌ (failure)

### **Step 3: Review Results**
- **Artifacts Tab**: Download test results & coverage reports
- **Summary**: See workflow execution time and status
- **Logs**: Click on individual steps to see detailed output

---

## 📋 What Gets Executed

```
┌─ Build & Test Job (Primary)
│  ├─ Checkout code
│  ├─ Setup .NET 10
│  ├─ Restore dependencies
│  ├─ Build solution (Release mode)
│  ├─ Setup SQL Server 2022
│  ├─ Run EF Core migrations
│  ├─ Execute 87 unit tests ⭐
│  ├─ Generate coverage report
│  └─ Upload artifacts
│
├─ Docker Build Job (If tests pass)
│  ├─ Setup Docker Buildx
│  ├─ Build Docker image
│  └─ Verify image
│
└─ Code Quality Job (Parallel)
   ├─ Check formatting
   └─ Build analysis
```

---

## 🎯 Expected Results (First Run)

### **Timeline**
```
Total Duration: ~5-7 minutes

Breakdown:
├─ Checkout:          15s
├─ Setup .NET:        30s
├─ Restore:           45s
├─ Build:             2m
├─ SQL Server setup:  30s
├─ Migrations:        20s
├─ Tests:             10.2s ⭐
├─ Coverage:          28.3s
└─ Upload artifacts:  10s
```

### **Expected Output**
```
✅ Build & Test Job       PASSED (4min 30s)
✅ Docker Build Job       PASSED (1min 15s)
✅ Code Quality Job       PASSED (45s)

Test Summary:
- Total Tests: 87
- Passed: 87 ✅
- Failed: 0 ❌
- Skipped: 0

Coverage Report: Generated (Cobertura XML format)
```

---

## 🔍 Where to Find Results

### **GitHub Actions Tab**
```
https://github.com/Callmesammy/Buy-Sell/actions
│
├── Latest run (green checkmark or red X)
│   ├── Summary (job status, duration)
│   ├── Artifacts (download test/coverage files)
│   └── Logs (click to expand each step)
│
└── Workflow history (all previous runs)
```

### **Workflow Status Indicators**

| Icon | Meaning |
|------|---------|
| 🟢 ✅ | All jobs passed |
| 🔴 ❌ | One or more jobs failed |
| 🟡 ⏳ | Currently running |
| ⚪ ⊘ | Skipped (conditional) |

---

## 📊 Key Files Created

```
Buy-Sell/
├── .github/
│   └── workflows/
│       └── ci-cd.yml                 ← Main workflow file
│
├── .github/
│   └── CI-CD_INTEGRATION_GUIDE.md     ← Complete documentation
│
└── .github/
    └── CI-CD_SETUP.md                 ← This file
```

---

## 🔧 Local Testing (Before GitHub Push)

### **Option 1: Run Tests Locally First**
```bash
cd "C:\Users\USER\source\repos\Buy&Sell"

# Build everything
dotnet build --configuration Release

# Run all tests
dotnet test --no-build --configuration Release

# Generate coverage
dotnet test --no-build --collect:"XPlat Code Coverage"
```

### **Option 2: Simulate GitHub Actions Locally**
```bash
# Install act (local GitHub Actions runner)
# https://github.com/nektos/act

act push --job test
```

---

## ✅ Verification Checklist

Before pushing to GitHub, verify:

- ✅ `.github/workflows/ci-cd.yml` exists
- ✅ File is properly formatted (YAML)
- ✅ Project builds locally: `dotnet build`
- ✅ Tests pass locally: `dotnet test`
- ✅ Docker file exists: `Dockerfile`
- ✅ Git is configured: `git config --global user.name`

---

## 🚨 Troubleshooting

### **❌ Workflow Not Triggering**

**Problem**: I pushed but workflow didn't run

**Solutions**:
```bash
# 1. Verify you pushed to correct branch
git log -1 --oneline origin/master

# 2. Check GitHub Actions is enabled
# Settings → Actions → General → Actions permissions = "Allow all actions"

# 3. Check workflow file syntax
# Use online YAML validator: https://www.yamllint.com/

# 4. Force re-run from GitHub UI
# Actions tab → Click workflow → "Re-run jobs"
```

### **❌ Tests Fail in GitHub but Pass Locally**

**Problem**: Tests work locally but fail in CI/CD

**Common Causes**:
1. **Database Connection**: Connection string not set correctly
   - Solution: Check SQL Server service in workflow is ready

2. **File Paths**: Absolute paths that don't exist on runner
   - Solution: Use relative paths or Environment.CurrentDirectory

3. **Timing Issues**: Tests too slow and timeout
   - Solution: Increase timeout values in workflow

4. **Missing Dependencies**: NuGet packages not restored
   - Solution: Verify `dotnet restore` step completes

---

## 🎯 Next Configuration Steps

### **1. Protect Main Branch** (Recommended)
```
GitHub Repo → Settings → Branches → Add rule
├─ Branch name pattern: master
├─ Require status checks to pass
│  └─ Select: ci-cd / Build & Test
└─ Require code reviews before merging
```

### **2. Add Coverage Requirements** (Optional)
```yaml
# Add to workflow before merge
- name: Check Coverage Threshold
  run: |
    coverage_percent=$(grep 'line-rate=' coverage.cobertura.xml | cut -d'"' -f2 | awk '{printf "%.0f", $1*100}')
    if [ $coverage_percent -lt 80 ]; then
      echo "Coverage too low: ${coverage_percent}%"
      exit 1
    fi
```

### **3. Configure Notifications** (Optional)
```bash
# Add to repo settings → Actions → Notifications
# Get alerts for workflow failures
```

---

## 🎓 Understanding the Workflow

### **Jobs Explained**

#### **Job 1: Build & Test** ⭐ (Critical)
```yaml
Runs on: ubuntu-latest (Linux container)
Duration: ~4-5 minutes

What it does:
├─ Compiles your .NET code
├─ Sets up SQL Server for tests
├─ Applies database migrations
├─ Executes all 87 unit tests
├─ Generates coverage metrics
└─ Archives results
```

#### **Job 2: Docker Build** (Deployment-ready)
```yaml
Runs on: ubuntu-latest
Depends on: Job 1 (only runs if tests pass)
Duration: ~1-2 minutes

What it does:
├─ Builds Docker image from Dockerfile
├─ Tags with commit SHA (e.g., abc123def)
├─ Tags with 'latest'
└─ Verifies image was created
```

#### **Job 3: Code Quality** (Maintenance)
```yaml
Runs on: ubuntu-latest (parallel to others)
Duration: ~1 minute

What it does:
├─ Checks code formatting
├─ Scans for compilation warnings
└─ Reports issues
```

---

## 📈 Monitoring & Analytics

### **View Test Trends**
```
GitHub Actions → Workflow → Click "Build & Test"
→ Scroll down to see:
├─ Execution history (duration over time)
├─ Success rate (green/red timeline)
└─ Average duration
```

### **Download Artifacts**
```
GitHub Actions → Latest run
→ Artifacts section
├─ test-results
│  └─ Contains: test-results.trx
└─ coverage-reports
   └─ Contains: coverage.cobertura.xml
```

### **Using Coverage Reports Locally**
```bash
# Download coverage.cobertura.xml from GitHub Artifacts

# Install ReportGenerator
dotnet tool install -g dotnet-reportgenerator-globaltool

# Generate HTML report
reportgenerator -reports:coverage.cobertura.xml -targetdir:html

# Open in browser
start html/index.htm
```

---

## 🔐 Security Notes

### **Current Setup** (Safe for Public)
```yaml
- No secrets exposed
- No credentials in code
- Test database isolated
- Temporary containers cleaned up
```

### **For Production Deployment** (Optional)
```bash
# If you want to deploy from CI/CD, add secrets:

GitHub Repo → Settings → Secrets and variables → Actions
├─ DOCKER_REGISTRY_URL
├─ DOCKER_REGISTRY_USERNAME
├─ DOCKER_REGISTRY_PASSWORD
├─ PRODUCTION_DATABASE_CONNECTION
└─ DEPLOY_TOKEN
```

---

## 📚 Useful Links

| Resource | URL |
|----------|-----|
| **Your Repo Actions** | https://github.com/Callmesammy/Buy-Sell/actions |
| **GitHub Actions Docs** | https://docs.github.com/en/actions |
| **Workflow Syntax** | https://docs.github.com/en/actions/using-workflows/workflow-syntax-for-github-actions |
| **xUnit on GitHub** | https://github.com/xunit/xunit |
| **Docker Hub** | https://hub.docker.com/_/microsoft-mssql-server |

---

## 🎉 Success!

Your CI/CD pipeline is now:
- ✅ **Automated**: Tests run on every push
- ✅ **Fast**: Complete in ~5-7 minutes
- ✅ **Reliable**: Consistent results
- ✅ **Visible**: Status visible on GitHub
- ✅ **Production-Ready**: Docker image built

---

## 📞 Quick Reference

### **Common Commands**
```bash
# Push changes to trigger workflow
git push origin master

# View workflow runs
# Visit: https://github.com/Callmesammy/Buy-Sell/actions

# Force re-run workflow
# GitHub UI → Actions → Workflow → "Re-run jobs"

# View detailed logs
# Click failed step in workflow run
```

### **Common Workflow Status**
| Status | Action |
|--------|--------|
| 🟢 All green | Everything working! |
| 🔴 Red test job | Check logs, fix test issue |
| 🔴 Red Docker job | Check Dockerfile or permissions |
| ⏳ Running | Wait for completion |

---

**Ready to use? Push to GitHub and monitor your first run! 🚀**

```bash
git push origin master
# Then go to: https://github.com/Callmesammy/Buy-Sell/actions
```
