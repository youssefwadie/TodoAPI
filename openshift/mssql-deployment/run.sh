# Create a project
oc new-project mssql

# Create secret
oc create secret generic mssql --from-literal=MSSQL_SA_PASSWORD="MyC0m9l&xP@ssw0rd"

# Storage
oc apply -f pvc.yaml

# Deployment
oc apply -f sqldeployment.yaml

