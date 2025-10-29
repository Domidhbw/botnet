# From the project root
(gc .\angular.json) -replace '(?m)^\s*"vendorChunk"\s*:\s*(true|false)\s*,?\s*$', '' |
    sc .\angular.json
