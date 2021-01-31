aws s3 sync ./dist s3://publicsitestack-publicsitebucket37ed9704-71kp5zq1swck --exclude index.html
aws s3 cp ./dist/index.html s3://publicsitestack-publicsitebucket37ed9704-71kp5zq1swck/index.html --cache-control "no-store, max-age=0"
