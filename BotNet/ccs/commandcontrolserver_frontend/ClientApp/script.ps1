# 0) Optional: new branch
git checkout -b chore/upgrade-ng17

# 1) Update core & CLI to 17 (uses the official migrations)
npx -p @angular/cli@17 ng update @angular/core@17 @angular/cli@17

# 2) Update Material & CDK to 17 (this removes the peer conflict)
npx ng update @angular/material@17

# 3) If you use Angular Universal, update it too
# (skip if you don't use @nguniversal/*)
npx ng update @nguniversal/express-engine@17 @nguniversal/builders@17

# 4) Ensure the build system is 17.x (Vite-based)
npm i -D @angular-devkit/build-angular@17.3.12 -E

# 5) Align compiler deps to what Angular 17 expects
npm i -D typescript@~5.2.2 zone.js@~0.14.3 -E

# 6) Clean install to avoid stale deps
Remove-Item -Recurse -Force node_modules, package-lock.json
npm ci

# 7) Verify no legacy webpack-dev-server remains
npm ls webpack-dev-server || $true
