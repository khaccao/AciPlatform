# AciPlatform API Documentation for Frontend

Generated at: 2026-03-13 11:57:26

## Overview

- API title: AciPlatform.Api
- API version: 1.0
- Base URL (dev): http://localhost:5092
- Swagger JSON source: artifacts/swagger-v1.json
- Auth probe report source: artifacts/api-tests/api-test-results.json
- Total paths: 136
- Total operations: 229
- Total schemas: 47
- Auth probe loaded: Yes

## Frontend Integration Notes

- Authentication uses Bearer JWT: send header Authorization: Bearer <token>.
- Main auth endpoints are in tag Auth (login/seed/refresh/me/logout).
- Many list endpoints follow paging query params (Page, PageSize, SearchText) from FilterParams.
- Auth columns in this document:
  - Auth (Swagger): derived from OpenAPI security metadata.
  - Auth (Observed): derived from unauthenticated API sweep status codes (401/403).
- Common status handling:
  - 200/201: success
  - 400: invalid request or business validation
  - 401: missing/expired token
  - 403: no permission
  - 404: entity not found
  - 500: unexpected server error

## Endpoint Index

### Allowances

| Method | Path | OperationId | Auth (Swagger) | Auth (Observed) |
|---|---|---|---|---|
| GET | /api/Allowances |  | No | Yes |
| POST | /api/Allowances |  | No | Yes |
| GET | /api/Allowances/{id} |  | No | Yes |
| PUT | /api/Allowances/{id} |  | No | Yes |
| DELETE | /api/Allowances/{id} |  | No | Yes |

### AllowanceUsers

| Method | Path | OperationId | Auth (Swagger) | Auth (Observed) |
|---|---|---|---|---|
| POST | /api/AllowanceUsers |  | No | Yes |
| GET | /api/AllowanceUsers/{id} |  | No | Yes |
| PUT | /api/AllowanceUsers/{id} |  | No | Yes |
| DELETE | /api/AllowanceUsers/{id} |  | No | Yes |
| GET | /api/AllowanceUsers/user/{userId} |  | No | Yes |

### Auth

| Method | Path | OperationId | Auth (Swagger) | Auth (Observed) |
|---|---|---|---|---|
| PUT | /api/Auth/change-password |  | No | Yes |
| GET | /api/Auth/debug-db |  | No | No |
| POST | /api/Auth/guess-login |  | No | No |
| POST | /api/Auth/invoice |  | No | No |
| POST | /api/Auth/login |  | No | No |
| POST | /api/Auth/refresh |  | No | Yes |
| POST | /api/Auth/register |  | No | No |
| POST | /api/Auth/requestForgotPass |  | No | No |
| POST | /api/Auth/seed |  | No | No |
| GET | /api/Auth/username-check |  | No | No |

### CarFields

| Method | Path | OperationId | Auth (Swagger) | Auth (Observed) |
|---|---|---|---|---|
| GET | /api/CarFields |  | No | Yes |
| POST | /api/CarFields |  | No | Yes |
| GET | /api/CarFields/{id} |  | No | Yes |
| PUT | /api/CarFields/{id} |  | No | Yes |
| DELETE | /api/CarFields/{id} |  | No | Yes |

### CarLocations

| Method | Path | OperationId | Auth (Swagger) | Auth (Observed) |
|---|---|---|---|---|
| GET | /api/CarLocations |  | No | Yes |
| POST | /api/CarLocations |  | No | Yes |
| GET | /api/CarLocations/{id} |  | No | Yes |
| PUT | /api/CarLocations/{id} |  | No | Yes |
| DELETE | /api/CarLocations/{id} |  | No | Yes |
| PUT | /api/CarLocations/accept/{id} |  | No | Yes |
| POST | /api/CarLocations/export/{id} |  | No | Yes |
| GET | /api/CarLocations/get-procedure-number |  | No | Yes |
| PUT | /api/CarLocations/not-accept/{id} |  | No | Yes |

### Cars

| Method | Path | OperationId | Auth (Swagger) | Auth (Observed) |
|---|---|---|---|---|
| GET | /api/Cars |  | No | Yes |
| POST | /api/Cars |  | No | Yes |
| GET | /api/Cars/{id} |  | No | Yes |
| PUT | /api/Cars/{id} |  | No | Yes |
| DELETE | /api/Cars/{id} |  | No | Yes |
| GET | /api/Cars/car-field-setup |  | No | Yes |
| PUT | /api/Cars/car-field-setup |  | No | Yes |
| GET | /api/Cars/list |  | No | Yes |

### Certificates

| Method | Path | OperationId | Auth (Swagger) | Auth (Observed) |
|---|---|---|---|---|
| GET | /api/Certificates |  | No | Yes |
| POST | /api/Certificates |  | No | Yes |
| GET | /api/Certificates/{id} |  | No | Yes |
| PUT | /api/Certificates/{id} |  | No | Yes |
| DELETE | /api/Certificates/{id} |  | No | Yes |
| GET | /api/Certificates/user/{userId} |  | No | Yes |

### Companies

| Method | Path | OperationId | Auth (Swagger) | Auth (Observed) |
|---|---|---|---|---|
| GET | /api/Companies |  | No | Yes |
| GET | /api/Companies/{code} |  | No | Yes |

### ContractFiles

| Method | Path | OperationId | Auth (Swagger) | Auth (Observed) |
|---|---|---|---|---|
| GET | /api/ContractFiles |  | No | Yes |
| POST | /api/ContractFiles |  | No | Yes |
| GET | /api/ContractFiles/{id} |  | No | Yes |
| PUT | /api/ContractFiles/{id} |  | No | Yes |
| DELETE | /api/ContractFiles/{id} |  | No | Yes |

### ContractTypes

| Method | Path | OperationId | Auth (Swagger) | Auth (Observed) |
|---|---|---|---|---|
| GET | /api/ContractTypes |  | No | Yes |
| POST | /api/ContractTypes |  | No | Yes |
| GET | /api/ContractTypes/{id} |  | No | Yes |
| PUT | /api/ContractTypes/{id} |  | No | Yes |
| DELETE | /api/ContractTypes/{id} |  | No | Yes |

### Decides

| Method | Path | OperationId | Auth (Swagger) | Auth (Observed) |
|---|---|---|---|---|
| GET | /api/Decides |  | No | Yes |
| POST | /api/Decides |  | No | Yes |
| GET | /api/Decides/{id} |  | No | Yes |
| PUT | /api/Decides/{id} |  | No | Yes |
| DELETE | /api/Decides/{id} |  | No | Yes |
| GET | /api/Decides/user/{userId} |  | No | Yes |

### DecisionTypes

| Method | Path | OperationId | Auth (Swagger) | Auth (Observed) |
|---|---|---|---|---|
| GET | /api/DecisionTypes |  | No | Yes |
| POST | /api/DecisionTypes |  | No | Yes |
| GET | /api/DecisionTypes/{id} |  | No | Yes |
| PUT | /api/DecisionTypes/{id} |  | No | Yes |
| DELETE | /api/DecisionTypes/{id} |  | No | Yes |

### Degrees

| Method | Path | OperationId | Auth (Swagger) | Auth (Observed) |
|---|---|---|---|---|
| GET | /api/Degrees |  | No | Yes |
| POST | /api/Degrees |  | No | Yes |
| GET | /api/Degrees/{id} |  | No | Yes |
| PUT | /api/Degrees/{id} |  | No | Yes |
| DELETE | /api/Degrees/{id} |  | No | Yes |
| GET | /api/Degrees/user/{userId} |  | No | Yes |

### Departments

| Method | Path | OperationId | Auth (Swagger) | Auth (Observed) |
|---|---|---|---|---|
| GET | /api/Departments |  | No | Yes |
| POST | /api/Departments |  | No | Yes |
| GET | /api/Departments/{id} |  | No | Yes |
| PUT | /api/Departments/{id} |  | No | Yes |
| DELETE | /api/Departments/{id} |  | No | Yes |

### DriverRouters

| Method | Path | OperationId | Auth (Swagger) | Auth (Observed) |
|---|---|---|---|---|
| GET | /api/DriverRouters |  | No | Yes |
| GET | /api/DriverRouters/{id} |  | No | Yes |
| PUT | /api/DriverRouters/{id} |  | No | Yes |
| DELETE | /api/DriverRouters/{id} |  | No | Yes |
| POST | /api/DriverRouters/finish |  | No | Yes |
| GET | /api/DriverRouters/list/police-point/{id} |  | No | Yes |
| POST | /api/DriverRouters/start |  | No | Yes |

### Facebook

| Method | Path | OperationId | Auth (Swagger) | Auth (Observed) |
|---|---|---|---|---|
| GET | /api/v1/multichannel/app-config |  | No | No |
| POST | /api/v1/multichannel/app-config |  | No | No |
| GET | /api/v1/multichannel/automation |  | No | No |
| POST | /api/v1/multichannel/automation |  | No | No |
| POST | /api/v1/multichannel/connect-page |  | No | No |
| POST | /api/v1/multichannel/disconnect-page/{pageId} |  | No | No |
| GET | /api/v1/multichannel/insights/{pageId} |  | No | No |
| POST | /api/v1/multichannel/oauth-callback |  | No | No |
| GET | /api/v1/multichannel/oauth-url |  | No | No |
| GET | /api/v1/multichannel/pages |  | No | No |
| POST | /api/v1/multichannel/post |  | No | No |
| POST | /api/v1/multichannel/publish |  | No | No |

### GoodWarehouseExports

| Method | Path | OperationId | Auth (Swagger) | Auth (Observed) |
|---|---|---|---|---|
| GET | /api/GoodWarehouseExports |  | No | Yes |
| DELETE | /api/GoodWarehouseExports/{billId} |  | No | Yes |

### GoodWarehouses

| Method | Path | OperationId | Auth (Swagger) | Auth (Observed) |
|---|---|---|---|---|
| GET | /api/GoodWarehouses |  | No | Yes |
| POST | /api/GoodWarehouses/sync |  | No | Yes |

### HistoryAchievements

| Method | Path | OperationId | Auth (Swagger) | Auth (Observed) |
|---|---|---|---|---|
| GET | /api/HistoryAchievements |  | No | Yes |
| POST | /api/HistoryAchievements |  | No | Yes |
| GET | /api/HistoryAchievements/{id} |  | No | Yes |
| PUT | /api/HistoryAchievements/{id} |  | No | Yes |
| DELETE | /api/HistoryAchievements/{id} |  | No | Yes |
| GET | /api/HistoryAchievements/user/{userId} |  | No | Yes |

### Inventories

| Method | Path | OperationId | Auth (Swagger) | Auth (Observed) |
|---|---|---|---|---|
| GET | /api/Inventories |  | No | Yes |
| POST | /api/Inventories |  | No | Yes |
| POST | /api/Inventories/accept |  | No | Yes |
| GET | /api/Inventories/list-date |  | No | Yes |
| GET | /api/Inventories/list-inventory |  | No | Yes |

### Majors

| Method | Path | OperationId | Auth (Swagger) | Auth (Observed) |
|---|---|---|---|---|
| GET | /api/Majors |  | No | Yes |
| POST | /api/Majors |  | No | Yes |
| GET | /api/Majors/{id} |  | No | Yes |
| PUT | /api/Majors/{id} |  | No | Yes |
| DELETE | /api/Majors/{id} |  | No | Yes |

### MenuRoles

| Method | Path | OperationId | Auth (Swagger) | Auth (Observed) |
|---|---|---|---|---|
| GET | /api/MenuRoles/role/{roleId} |  | No | Yes |
| POST | /api/MenuRoles/role/{roleId} |  | No | Yes |

### Menus

| Method | Path | OperationId | Auth (Swagger) | Auth (Observed) |
|---|---|---|---|---|
| GET | /api/Menus |  | No | Yes |
| POST | /api/Menus |  | No | Yes |
| GET | /api/Menus/{id} |  | No | Yes |
| PUT | /api/Menus/{id} |  | No | Yes |
| DELETE | /api/Menus/{id} |  | No | Yes |
| GET | /api/Menus/check-role |  | No | Yes |
| GET | /api/Menus/list |  | No | Yes |
| GET | /api/Menus/user/{userId} |  | No | Yes |
| GET | /api/Menus/user/{userId}/permissions |  | No | Yes |

### PetrolConsumptions

| Method | Path | OperationId | Auth (Swagger) | Auth (Observed) |
|---|---|---|---|---|
| GET | /api/PetrolConsumptions |  | No | Yes |
| POST | /api/PetrolConsumptions |  | No | Yes |
| GET | /api/PetrolConsumptions/{id} |  | No | Yes |
| PUT | /api/PetrolConsumptions/{id} |  | No | Yes |
| DELETE | /api/PetrolConsumptions/{id} |  | No | Yes |
| GET | /api/PetrolConsumptions/report |  | No | Yes |

### PoliceCheckPoints

| Method | Path | OperationId | Auth (Swagger) | Auth (Observed) |
|---|---|---|---|---|
| GET | /api/PoliceCheckPoints |  | No | Yes |
| POST | /api/PoliceCheckPoints |  | No | Yes |
| GET | /api/PoliceCheckPoints/{id} |  | No | Yes |
| PUT | /api/PoliceCheckPoints/{id} |  | No | Yes |
| DELETE | /api/PoliceCheckPoints/{id} |  | No | Yes |
| GET | /api/PoliceCheckPoints/list |  | No | Yes |

### PositionDetails

| Method | Path | OperationId | Auth (Swagger) | Auth (Observed) |
|---|---|---|---|---|
| GET | /api/PositionDetails |  | No | Yes |
| POST | /api/PositionDetails |  | No | Yes |
| GET | /api/PositionDetails/{id} |  | No | Yes |
| PUT | /api/PositionDetails/{id} |  | No | Yes |
| DELETE | /api/PositionDetails/{id} |  | No | Yes |

### Relatives

| Method | Path | OperationId | Auth (Swagger) | Auth (Observed) |
|---|---|---|---|---|
| GET | /api/Relatives |  | No | Yes |
| POST | /api/Relatives |  | No | Yes |
| GET | /api/Relatives/{id} |  | No | Yes |
| PUT | /api/Relatives/{id} |  | No | Yes |
| DELETE | /api/Relatives/{id} |  | No | Yes |
| GET | /api/Relatives/user/{userId} |  | No | Yes |

### RoadRoutes

| Method | Path | OperationId | Auth (Swagger) | Auth (Observed) |
|---|---|---|---|---|
| GET | /api/RoadRoutes |  | No | Yes |
| POST | /api/RoadRoutes |  | No | Yes |
| GET | /api/RoadRoutes/{id} |  | No | Yes |
| PUT | /api/RoadRoutes/{id} |  | No | Yes |
| DELETE | /api/RoadRoutes/{id} |  | No | Yes |

### SalaryTypes

| Method | Path | OperationId | Auth (Swagger) | Auth (Observed) |
|---|---|---|---|---|
| GET | /api/SalaryTypes |  | No | Yes |
| POST | /api/SalaryTypes |  | No | Yes |
| GET | /api/SalaryTypes/{id} |  | No | Yes |
| PUT | /api/SalaryTypes/{id} |  | No | Yes |
| DELETE | /api/SalaryTypes/{id} |  | No | Yes |

### TimeKeeping

| Method | Path | OperationId | Auth (Swagger) | Auth (Observed) |
|---|---|---|---|---|
| GET | /api/TimeKeeping |  | No | Yes |
| POST | /api/TimeKeeping |  | No | Yes |
| GET | /api/TimeKeeping/{id} |  | No | Yes |
| PUT | /api/TimeKeeping/{id} |  | No | Yes |
| DELETE | /api/TimeKeeping/{id} |  | No | Yes |

### UserContractHistories

| Method | Path | OperationId | Auth (Swagger) | Auth (Observed) |
|---|---|---|---|---|
| GET | /api/UserContractHistories |  | No | Yes |
| POST | /api/UserContractHistories |  | No | Yes |
| GET | /api/UserContractHistories/{id} |  | No | Yes |
| PUT | /api/UserContractHistories/{id} |  | No | Yes |
| DELETE | /api/UserContractHistories/{id} |  | No | Yes |
| GET | /api/UserContractHistories/user/{userId} |  | No | Yes |

### UserMenus

| Method | Path | OperationId | Auth (Swagger) | Auth (Observed) |
|---|---|---|---|---|
| GET | /api/UserMenus/{userId} |  | No | Yes |
| POST | /api/UserMenus/{userId}/assign |  | No | Yes |
| DELETE | /api/UserMenus/{userId}/clear |  | No | Yes |
| DELETE | /api/UserMenus/{userId}/menu/{menuId} |  | No | Yes |
| GET | /api/UserMenus/{userId}/permissions |  | No | Yes |

### UserRoles

| Method | Path | OperationId | Auth (Swagger) | Auth (Observed) |
|---|---|---|---|---|
| GET | /api/UserRoles |  | No | Yes |
| POST | /api/UserRoles |  | No | Yes |
| GET | /api/UserRoles/{id} |  | No | Yes |
| PUT | /api/UserRoles/{id} |  | No | Yes |
| DELETE | /api/UserRoles/{id} |  | No | Yes |
| GET | /api/UserRoles/list |  | No | Yes |

### Users

| Method | Path | OperationId | Auth (Swagger) | Auth (Observed) |
|---|---|---|---|---|
| GET | /api/Users |  | No | Yes |
| POST | /api/Users |  | No | Yes |
| GET | /api/Users/{id} |  | No | Yes |
| PUT | /api/Users/{id} |  | No | Yes |
| DELETE | /api/Users/{id} |  | No | Yes |
| GET | /api/Users/getAllUserActive |  | No | Yes |
| POST | /api/Users/getallusername |  | No | Yes |
| GET | /api/Users/get-total-reset-pass |  | No | Yes |
| POST | /api/Users/resetPassword |  | No | Yes |
| PUT | /api/Users/update-current-year |  | No | Yes |
| GET | /api/Users/user-not-roles |  | No | Yes |

### WareHouseFloors

| Method | Path | OperationId | Auth (Swagger) | Auth (Observed) |
|---|---|---|---|---|
| GET | /api/WareHouseFloors |  | No | Yes |
| POST | /api/WareHouseFloors |  | No | Yes |
| GET | /api/WareHouseFloors/{id} |  | No | Yes |
| PUT | /api/WareHouseFloors/{id} |  | No | Yes |
| DELETE | /api/WareHouseFloors/{id} |  | No | Yes |
| GET | /api/WareHouseFloors/list |  | No | Yes |

### WareHousePositions

| Method | Path | OperationId | Auth (Swagger) | Auth (Observed) |
|---|---|---|---|---|
| GET | /api/WareHousePositions |  | No | Yes |
| POST | /api/WareHousePositions |  | No | Yes |
| GET | /api/WareHousePositions/{id} |  | No | Yes |
| PUT | /api/WareHousePositions/{id} |  | No | Yes |
| DELETE | /api/WareHousePositions/{id} |  | No | Yes |
| GET | /api/WareHousePositions/list |  | No | Yes |

### Warehouses

| Method | Path | OperationId | Auth (Swagger) | Auth (Observed) |
|---|---|---|---|---|
| GET | /api/Warehouses |  | No | Yes |
| POST | /api/Warehouses |  | No | Yes |
| GET | /api/Warehouses/{id} |  | No | Yes |
| PUT | /api/Warehouses/{id} |  | No | Yes |
| DELETE | /api/Warehouses/{id} |  | No | Yes |
| GET | /api/Warehouses/list |  | No | Yes |

### WareHouseShelves

| Method | Path | OperationId | Auth (Swagger) | Auth (Observed) |
|---|---|---|---|---|
| GET | /api/WareHouseShelves |  | No | Yes |
| POST | /api/WareHouseShelves |  | No | Yes |
| GET | /api/WareHouseShelves/{id} |  | No | Yes |
| PUT | /api/WareHouseShelves/{id} |  | No | Yes |
| DELETE | /api/WareHouseShelves/{id} |  | No | Yes |
| GET | /api/WareHouseShelves/list |  | No | Yes |

### WebAuth

| Method | Path | OperationId | Auth (Swagger) | Auth (Observed) |
|---|---|---|---|---|
| POST | /api/WebAuth/change-pass-word/{id} |  | No | Yes |
| POST | /api/WebAuth/info |  | No | Yes |
| GET | /api/WebAuth/info/{id} |  | No | Yes |
| POST | /api/WebAuth/login |  | No | Yes |
| POST | /api/WebAuth/loginsocial |  | No | No |
| POST | /api/WebAuth/register |  | No | No |
| POST | /api/WebAuth/update-email |  | No | No |

## Detailed Endpoint Reference

## Tag: Allowances

### GET /api/Allowances

- OperationId: 
- Auth (Swagger): No
- Auth (Observed): Yes

#### Responses

| Status | Description | Content-Type | Schema |
|---|---|---|---|
| 200 | Success |  |  |

### POST /api/Allowances

- OperationId: 
- Auth (Swagger): No
- Auth (Observed): Yes

#### Request Body

- Required: No

| Content-Type | Schema |
|---|---|
| application/json | AllowanceRequest |
| text/json | AllowanceRequest |
| application/*+json | AllowanceRequest |

Sample JSON request:

```json
{
    "name":  "sample",
    "code":  "sample",
    "amount":  1,
    "description":  "sample"
}
```

#### Responses

| Status | Description | Content-Type | Schema |
|---|---|---|---|
| 200 | Success |  |  |

### GET /api/Allowances/{id}

- OperationId: 
- Auth (Swagger): No
- Auth (Observed): Yes

#### Parameters

| Name | In | Required | Type | Description |
|---|---|---|---|---|
| id | path | Yes | integer(int32) |  |

#### Responses

| Status | Description | Content-Type | Schema |
|---|---|---|---|
| 200 | Success |  |  |

### PUT /api/Allowances/{id}

- OperationId: 
- Auth (Swagger): No
- Auth (Observed): Yes

#### Parameters

| Name | In | Required | Type | Description |
|---|---|---|---|---|
| id | path | Yes | integer(int32) |  |

#### Request Body

- Required: No

| Content-Type | Schema |
|---|---|
| application/json | AllowanceRequest |
| text/json | AllowanceRequest |
| application/*+json | AllowanceRequest |

Sample JSON request:

```json
{
    "name":  "sample",
    "code":  "sample",
    "amount":  1,
    "description":  "sample"
}
```

#### Responses

| Status | Description | Content-Type | Schema |
|---|---|---|---|
| 200 | Success |  |  |

### DELETE /api/Allowances/{id}

- OperationId: 
- Auth (Swagger): No
- Auth (Observed): Yes

#### Parameters

| Name | In | Required | Type | Description |
|---|---|---|---|---|
| id | path | Yes | integer(int32) |  |

#### Responses

| Status | Description | Content-Type | Schema |
|---|---|---|---|
| 200 | Success |  |  |

## Tag: AllowanceUsers

### POST /api/AllowanceUsers

- OperationId: 
- Auth (Swagger): No
- Auth (Observed): Yes

#### Request Body

- Required: No

| Content-Type | Schema |
|---|---|
| application/json | AllowanceUserRequest |
| text/json | AllowanceUserRequest |
| application/*+json | AllowanceUserRequest |

Sample JSON request:

```json
{
    "allowanceId":  1,
    "userId":  1,
    "startDate":  "2026-03-13T11:57:26.7674410+07:00",
    "endDate":  "2026-03-13T11:57:26.7755186+07:00",
    "amountOverride":  1,
    "note":  "sample"
}
```

#### Responses

| Status | Description | Content-Type | Schema |
|---|---|---|---|
| 200 | Success |  |  |

### GET /api/AllowanceUsers/{id}

- OperationId: 
- Auth (Swagger): No
- Auth (Observed): Yes

#### Parameters

| Name | In | Required | Type | Description |
|---|---|---|---|---|
| id | path | Yes | integer(int32) |  |

#### Responses

| Status | Description | Content-Type | Schema |
|---|---|---|---|
| 200 | Success |  |  |

### PUT /api/AllowanceUsers/{id}

- OperationId: 
- Auth (Swagger): No
- Auth (Observed): Yes

#### Parameters

| Name | In | Required | Type | Description |
|---|---|---|---|---|
| id | path | Yes | integer(int32) |  |

#### Request Body

- Required: No

| Content-Type | Schema |
|---|---|
| application/json | AllowanceUserRequest |
| text/json | AllowanceUserRequest |
| application/*+json | AllowanceUserRequest |

Sample JSON request:

```json
{
    "allowanceId":  1,
    "userId":  1,
    "startDate":  "2026-03-13T11:57:26.7827295+07:00",
    "endDate":  "2026-03-13T11:57:26.7829956+07:00",
    "amountOverride":  1,
    "note":  "sample"
}
```

#### Responses

| Status | Description | Content-Type | Schema |
|---|---|---|---|
| 200 | Success |  |  |

### DELETE /api/AllowanceUsers/{id}

- OperationId: 
- Auth (Swagger): No
- Auth (Observed): Yes

#### Parameters

| Name | In | Required | Type | Description |
|---|---|---|---|---|
| id | path | Yes | integer(int32) |  |

#### Responses

| Status | Description | Content-Type | Schema |
|---|---|---|---|
| 200 | Success |  |  |

### GET /api/AllowanceUsers/user/{userId}

- OperationId: 
- Auth (Swagger): No
- Auth (Observed): Yes

#### Parameters

| Name | In | Required | Type | Description |
|---|---|---|---|---|
| userId | path | Yes | integer(int32) |  |

#### Responses

| Status | Description | Content-Type | Schema |
|---|---|---|---|
| 200 | Success |  |  |

## Tag: Auth

### PUT /api/Auth/change-password

- OperationId: 
- Auth (Swagger): No
- Auth (Observed): Yes

#### Request Body

- Required: No

| Content-Type | Schema |
|---|---|
| application/json | ChangePasswordRequest |
| text/json | ChangePasswordRequest |
| application/*+json | ChangePasswordRequest |

Sample JSON request:

```json
{
    "id":  1,
    "oldPassword":  "sample",
    "newPassword":  "sample"
}
```

#### Responses

| Status | Description | Content-Type | Schema |
|---|---|---|---|
| 200 | Success |  |  |

### GET /api/Auth/debug-db

- OperationId: 
- Auth (Swagger): No
- Auth (Observed): No

#### Responses

| Status | Description | Content-Type | Schema |
|---|---|---|---|
| 200 | Success |  |  |

### POST /api/Auth/guess-login

- OperationId: 
- Auth (Swagger): No
- Auth (Observed): No

#### Responses

| Status | Description | Content-Type | Schema |
|---|---|---|---|
| 200 | Success |  |  |

### POST /api/Auth/invoice

- OperationId: 
- Auth (Swagger): No
- Auth (Observed): No

#### Parameters

| Name | In | Required | Type | Description |
|---|---|---|---|---|
| billId | query | No | integer(int32) |  |

#### Responses

| Status | Description | Content-Type | Schema |
|---|---|---|---|
| 200 | Success |  |  |

### POST /api/Auth/login

- OperationId: 
- Auth (Swagger): No
- Auth (Observed): No

#### Request Body

- Required: No

| Content-Type | Schema |
|---|---|
| application/json | AuthenticateModel |
| text/json | AuthenticateModel |
| application/*+json | AuthenticateModel |

Sample JSON request:

```json
{
    "username":  "sample",
    "password":  "sample",
    "companyCode":  "sample"
}
```

#### Responses

| Status | Description | Content-Type | Schema |
|---|---|---|---|
| 200 | Success |  |  |

### POST /api/Auth/refresh

- OperationId: 
- Auth (Swagger): No
- Auth (Observed): Yes

#### Responses

| Status | Description | Content-Type | Schema |
|---|---|---|---|
| 200 | Success |  |  |

### POST /api/Auth/register

- OperationId: 
- Auth (Swagger): No
- Auth (Observed): No

#### Request Body

- Required: No

| Content-Type | Schema |
|---|---|
| application/json | RegisterRequest |
| text/json | RegisterRequest |
| application/*+json | RegisterRequest |

Sample JSON request:

```json
{
    "username":  "sample",
    "password":  "sample",
    "fullName":  "sample",
    "email":  "sample",
    "userRoleIds":  "sample",
    "companyCode":  "sample"
}
```

#### Responses

| Status | Description | Content-Type | Schema |
|---|---|---|---|
| 200 | Success |  |  |

### POST /api/Auth/requestForgotPass

- OperationId: 
- Auth (Swagger): No
- Auth (Observed): No

#### Request Body

- Required: No

| Content-Type | Schema |
|---|---|
| application/json | ForgotPasswordRequest |
| text/json | ForgotPasswordRequest |
| application/*+json | ForgotPasswordRequest |

Sample JSON request:

```json
{
    "username":  "sample"
}
```

#### Responses

| Status | Description | Content-Type | Schema |
|---|---|---|---|
| 200 | Success |  |  |

### POST /api/Auth/seed

- OperationId: 
- Auth (Swagger): No
- Auth (Observed): No

#### Responses

| Status | Description | Content-Type | Schema |
|---|---|---|---|
| 200 | Success |  |  |

### GET /api/Auth/username-check

- OperationId: 
- Auth (Swagger): No
- Auth (Observed): No

#### Parameters

| Name | In | Required | Type | Description |
|---|---|---|---|---|
| username | query | No | string |  |

#### Responses

| Status | Description | Content-Type | Schema |
|---|---|---|---|
| 200 | Success |  |  |

## Tag: CarFields

### GET /api/CarFields

- OperationId: 
- Auth (Swagger): No
- Auth (Observed): Yes

#### Parameters

| Name | In | Required | Type | Description |
|---|---|---|---|---|
| Page | query | No | integer(int32) |  |
| PageSize | query | No | integer(int32) |  |
| SearchText | query | No | string |  |

#### Responses

| Status | Description | Content-Type | Schema |
|---|---|---|---|
| 200 | Success |  |  |

### POST /api/CarFields

- OperationId: 
- Auth (Swagger): No
- Auth (Observed): Yes

#### Request Body

- Required: No

| Content-Type | Schema |
|---|---|
| application/json | CarFieldModel |
| text/json | CarFieldModel |
| application/*+json | CarFieldModel |

Sample JSON request:

```json
{
    "id":  1,
    "carId":  1,
    "order":  1,
    "name":  "sample"
}
```

#### Responses

| Status | Description | Content-Type | Schema |
|---|---|---|---|
| 200 | Success |  |  |

### GET /api/CarFields/{id}

- OperationId: 
- Auth (Swagger): No
- Auth (Observed): Yes

#### Parameters

| Name | In | Required | Type | Description |
|---|---|---|---|---|
| id | path | Yes | integer(int32) |  |

#### Responses

| Status | Description | Content-Type | Schema |
|---|---|---|---|
| 200 | Success |  |  |

### PUT /api/CarFields/{id}

- OperationId: 
- Auth (Swagger): No
- Auth (Observed): Yes

#### Parameters

| Name | In | Required | Type | Description |
|---|---|---|---|---|
| id | path | Yes | integer(int32) |  |

#### Request Body

- Required: No

| Content-Type | Schema |
|---|---|
| application/json | CarFieldModel |
| text/json | CarFieldModel |
| application/*+json | CarFieldModel |

Sample JSON request:

```json
{
    "id":  1,
    "carId":  1,
    "order":  1,
    "name":  "sample"
}
```

#### Responses

| Status | Description | Content-Type | Schema |
|---|---|---|---|
| 200 | Success |  |  |

### DELETE /api/CarFields/{id}

- OperationId: 
- Auth (Swagger): No
- Auth (Observed): Yes

#### Parameters

| Name | In | Required | Type | Description |
|---|---|---|---|---|
| id | path | Yes | integer(int32) |  |

#### Responses

| Status | Description | Content-Type | Schema |
|---|---|---|---|
| 200 | Success |  |  |

## Tag: CarLocations

### GET /api/CarLocations

- OperationId: 
- Auth (Swagger): No
- Auth (Observed): Yes

#### Parameters

| Name | In | Required | Type | Description |
|---|---|---|---|---|
| Page | query | No | integer(int32) |  |
| PageSize | query | No | integer(int32) |  |
| SearchText | query | No | string |  |

#### Responses

| Status | Description | Content-Type | Schema |
|---|---|---|---|
| 200 | Success |  |  |

### POST /api/CarLocations

- OperationId: 
- Auth (Swagger): No
- Auth (Observed): Yes

#### Request Body

- Required: No

| Content-Type | Schema |
|---|---|
| application/json | CarLocationModel |
| text/json | CarLocationModel |
| application/*+json | CarLocationModel |

Sample JSON request:

```json
{
    "id":  1,
    "date":  "2026-03-13T11:57:26.8407623+07:00",
    "note":  "sample",
    "procedureNumber":  "sample",
    "status":  "sample",
    "details":  {
                    "id":  1,
                    "carLocationId":  1,
                    "licensePlates":  "sample",
                    "type":  "sample",
                    "payload":  "sample",
                    "driverName":  "sample",
                    "location":  "sample",
                    "planInprogress":  "sample",
                    "planExpected":  "sample",
                    "note":  "sample",
                    "fileStr":  "sample"
                }
}
```

#### Responses

| Status | Description | Content-Type | Schema |
|---|---|---|---|
| 200 | Success |  |  |

### GET /api/CarLocations/{id}

- OperationId: 
- Auth (Swagger): No
- Auth (Observed): Yes

#### Parameters

| Name | In | Required | Type | Description |
|---|---|---|---|---|
| id | path | Yes | integer(int32) |  |

#### Responses

| Status | Description | Content-Type | Schema |
|---|---|---|---|
| 200 | Success |  |  |

### PUT /api/CarLocations/{id}

- OperationId: 
- Auth (Swagger): No
- Auth (Observed): Yes

#### Parameters

| Name | In | Required | Type | Description |
|---|---|---|---|---|
| id | path | Yes | integer(int32) |  |

#### Request Body

- Required: No

| Content-Type | Schema |
|---|---|
| application/json | CarLocationModel |
| text/json | CarLocationModel |
| application/*+json | CarLocationModel |

Sample JSON request:

```json
{
    "id":  1,
    "date":  "2026-03-13T11:57:26.8568557+07:00",
    "note":  "sample",
    "procedureNumber":  "sample",
    "status":  "sample",
    "details":  {
                    "id":  1,
                    "carLocationId":  1,
                    "licensePlates":  "sample",
                    "type":  "sample",
                    "payload":  "sample",
                    "driverName":  "sample",
                    "location":  "sample",
                    "planInprogress":  "sample",
                    "planExpected":  "sample",
                    "note":  "sample",
                    "fileStr":  "sample"
                }
}
```

#### Responses

| Status | Description | Content-Type | Schema |
|---|---|---|---|
| 200 | Success |  |  |

### DELETE /api/CarLocations/{id}

- OperationId: 
- Auth (Swagger): No
- Auth (Observed): Yes

#### Parameters

| Name | In | Required | Type | Description |
|---|---|---|---|---|
| id | path | Yes | integer(int32) |  |

#### Responses

| Status | Description | Content-Type | Schema |
|---|---|---|---|
| 200 | Success |  |  |

### PUT /api/CarLocations/accept/{id}

- OperationId: 
- Auth (Swagger): No
- Auth (Observed): Yes

#### Parameters

| Name | In | Required | Type | Description |
|---|---|---|---|---|
| id | path | Yes | integer(int32) |  |

#### Responses

| Status | Description | Content-Type | Schema |
|---|---|---|---|
| 200 | Success |  |  |

### POST /api/CarLocations/export/{id}

- OperationId: 
- Auth (Swagger): No
- Auth (Observed): Yes

#### Parameters

| Name | In | Required | Type | Description |
|---|---|---|---|---|
| id | path | Yes | integer(int32) |  |

#### Responses

| Status | Description | Content-Type | Schema |
|---|---|---|---|
| 200 | Success |  |  |

### GET /api/CarLocations/get-procedure-number

- OperationId: 
- Auth (Swagger): No
- Auth (Observed): Yes

#### Responses

| Status | Description | Content-Type | Schema |
|---|---|---|---|
| 200 | Success |  |  |

### PUT /api/CarLocations/not-accept/{id}

- OperationId: 
- Auth (Swagger): No
- Auth (Observed): Yes

#### Parameters

| Name | In | Required | Type | Description |
|---|---|---|---|---|
| id | path | Yes | integer(int32) |  |

#### Responses

| Status | Description | Content-Type | Schema |
|---|---|---|---|
| 200 | Success |  |  |

## Tag: Cars

### GET /api/Cars

- OperationId: 
- Auth (Swagger): No
- Auth (Observed): Yes

#### Parameters

| Name | In | Required | Type | Description |
|---|---|---|---|---|
| Page | query | No | integer(int32) |  |
| PageSize | query | No | integer(int32) |  |
| SearchText | query | No | string |  |

#### Responses

| Status | Description | Content-Type | Schema |
|---|---|---|---|
| 200 | Success |  |  |

### POST /api/Cars

- OperationId: 
- Auth (Swagger): No
- Auth (Observed): Yes

#### Request Body

- Required: No

| Content-Type | Schema |
|---|---|
| application/json | CarModel |
| text/json | CarModel |
| application/*+json | CarModel |

Sample JSON request:

```json
{
    "id":  1,
    "licensePlates":  "sample",
    "note":  "sample",
    "content":  "sample",
    "mileageAllowance":  1,
    "fuelAmount":  1,
    "files":  "sample"
}
```

#### Responses

| Status | Description | Content-Type | Schema |
|---|---|---|---|
| 200 | Success |  |  |

### GET /api/Cars/{id}

- OperationId: 
- Auth (Swagger): No
- Auth (Observed): Yes

#### Parameters

| Name | In | Required | Type | Description |
|---|---|---|---|---|
| id | path | Yes | integer(int32) |  |

#### Responses

| Status | Description | Content-Type | Schema |
|---|---|---|---|
| 200 | Success |  |  |

### PUT /api/Cars/{id}

- OperationId: 
- Auth (Swagger): No
- Auth (Observed): Yes

#### Parameters

| Name | In | Required | Type | Description |
|---|---|---|---|---|
| id | path | Yes | integer(int32) |  |

#### Request Body

- Required: No

| Content-Type | Schema |
|---|---|
| application/json | CarModel |
| text/json | CarModel |
| application/*+json | CarModel |

Sample JSON request:

```json
{
    "id":  1,
    "licensePlates":  "sample",
    "note":  "sample",
    "content":  "sample",
    "mileageAllowance":  1,
    "fuelAmount":  1,
    "files":  "sample"
}
```

#### Responses

| Status | Description | Content-Type | Schema |
|---|---|---|---|
| 200 | Success |  |  |

### DELETE /api/Cars/{id}

- OperationId: 
- Auth (Swagger): No
- Auth (Observed): Yes

#### Parameters

| Name | In | Required | Type | Description |
|---|---|---|---|---|
| id | path | Yes | integer(int32) |  |

#### Responses

| Status | Description | Content-Type | Schema |
|---|---|---|---|
| 200 | Success |  |  |

### GET /api/Cars/car-field-setup

- OperationId: 
- Auth (Swagger): No
- Auth (Observed): Yes

#### Parameters

| Name | In | Required | Type | Description |
|---|---|---|---|---|
| carId | query | No | integer(int32) |  |

#### Responses

| Status | Description | Content-Type | Schema |
|---|---|---|---|
| 200 | Success |  |  |

### PUT /api/Cars/car-field-setup

- OperationId: 
- Auth (Swagger): No
- Auth (Observed): Yes

#### Parameters

| Name | In | Required | Type | Description |
|---|---|---|---|---|
| carId | query | No | integer(int32) |  |

#### Request Body

- Required: No

| Content-Type | Schema |
|---|---|
| application/json | array<CarFieldSetupModel> |
| text/json | array<CarFieldSetupModel> |
| application/*+json | array<CarFieldSetupModel> |

Sample JSON request:

```json
{
    "id":  1,
    "carId":  1,
    "carFieldId":  1,
    "valueNumber":  1,
    "fromAt":  "2026-03-13T11:57:26.9098187+07:00",
    "toAt":  "2026-03-13T11:57:26.9098187+07:00",
    "warningAt":  "2026-03-13T11:57:26.9098187+07:00",
    "userIdString":  "sample",
    "note":  "sample",
    "fileLink":  "sample"
}
```

#### Responses

| Status | Description | Content-Type | Schema |
|---|---|---|---|
| 200 | Success |  |  |

### GET /api/Cars/list

- OperationId: 
- Auth (Swagger): No
- Auth (Observed): Yes

#### Responses

| Status | Description | Content-Type | Schema |
|---|---|---|---|
| 200 | Success |  |  |

## Tag: Certificates

### GET /api/Certificates

- OperationId: 
- Auth (Swagger): No
- Auth (Observed): Yes

#### Responses

| Status | Description | Content-Type | Schema |
|---|---|---|---|
| 200 | Success |  |  |

### POST /api/Certificates

- OperationId: 
- Auth (Swagger): No
- Auth (Observed): Yes

#### Request Body

- Required: No

| Content-Type | Schema |
|---|---|
| application/json | CertificateRequest |
| text/json | CertificateRequest |
| application/*+json | CertificateRequest |

Sample JSON request:

```json
{
    "userId":  1,
    "name":  "sample",
    "issuer":  "sample",
    "issueDate":  "2026-03-13T11:57:26.9281616+07:00",
    "expiryDate":  "2026-03-13T11:57:26.9281616+07:00",
    "note":  "sample"
}
```

#### Responses

| Status | Description | Content-Type | Schema |
|---|---|---|---|
| 200 | Success |  |  |

### GET /api/Certificates/{id}

- OperationId: 
- Auth (Swagger): No
- Auth (Observed): Yes

#### Parameters

| Name | In | Required | Type | Description |
|---|---|---|---|---|
| id | path | Yes | integer(int32) |  |

#### Responses

| Status | Description | Content-Type | Schema |
|---|---|---|---|
| 200 | Success |  |  |

### PUT /api/Certificates/{id}

- OperationId: 
- Auth (Swagger): No
- Auth (Observed): Yes

#### Parameters

| Name | In | Required | Type | Description |
|---|---|---|---|---|
| id | path | Yes | integer(int32) |  |

#### Request Body

- Required: No

| Content-Type | Schema |
|---|---|
| application/json | CertificateRequest |
| text/json | CertificateRequest |
| application/*+json | CertificateRequest |

Sample JSON request:

```json
{
    "userId":  1,
    "name":  "sample",
    "issuer":  "sample",
    "issueDate":  "2026-03-13T11:57:26.9341674+07:00",
    "expiryDate":  "2026-03-13T11:57:26.9341674+07:00",
    "note":  "sample"
}
```

#### Responses

| Status | Description | Content-Type | Schema |
|---|---|---|---|
| 200 | Success |  |  |

### DELETE /api/Certificates/{id}

- OperationId: 
- Auth (Swagger): No
- Auth (Observed): Yes

#### Parameters

| Name | In | Required | Type | Description |
|---|---|---|---|---|
| id | path | Yes | integer(int32) |  |

#### Responses

| Status | Description | Content-Type | Schema |
|---|---|---|---|
| 200 | Success |  |  |

### GET /api/Certificates/user/{userId}

- OperationId: 
- Auth (Swagger): No
- Auth (Observed): Yes

#### Parameters

| Name | In | Required | Type | Description |
|---|---|---|---|---|
| userId | path | Yes | integer(int32) |  |

#### Responses

| Status | Description | Content-Type | Schema |
|---|---|---|---|
| 200 | Success |  |  |

## Tag: Companies

### GET /api/Companies

- OperationId: 
- Auth (Swagger): No
- Auth (Observed): Yes

#### Responses

| Status | Description | Content-Type | Schema |
|---|---|---|---|
| 200 | Success |  |  |

### GET /api/Companies/{code}

- OperationId: 
- Auth (Swagger): No
- Auth (Observed): Yes

#### Parameters

| Name | In | Required | Type | Description |
|---|---|---|---|---|
| code | path | Yes | string |  |

#### Responses

| Status | Description | Content-Type | Schema |
|---|---|---|---|
| 200 | Success |  |  |

## Tag: ContractFiles

### GET /api/ContractFiles

- OperationId: 
- Auth (Swagger): No
- Auth (Observed): Yes

#### Responses

| Status | Description | Content-Type | Schema |
|---|---|---|---|
| 200 | Success |  |  |

### POST /api/ContractFiles

- OperationId: 
- Auth (Swagger): No
- Auth (Observed): Yes

#### Request Body

- Required: No

| Content-Type | Schema |
|---|---|
| application/json | ContractFileRequest |
| text/json | ContractFileRequest |
| application/*+json | ContractFileRequest |

Sample JSON request:

```json
{
    "name":  "sample",
    "fileUrl":  "sample",
    "contractTypeId":  1,
    "note":  "sample"
}
```

#### Responses

| Status | Description | Content-Type | Schema |
|---|---|---|---|
| 200 | Success |  |  |

### GET /api/ContractFiles/{id}

- OperationId: 
- Auth (Swagger): No
- Auth (Observed): Yes

#### Parameters

| Name | In | Required | Type | Description |
|---|---|---|---|---|
| id | path | Yes | integer(int32) |  |

#### Responses

| Status | Description | Content-Type | Schema |
|---|---|---|---|
| 200 | Success |  |  |

### PUT /api/ContractFiles/{id}

- OperationId: 
- Auth (Swagger): No
- Auth (Observed): Yes

#### Parameters

| Name | In | Required | Type | Description |
|---|---|---|---|---|
| id | path | Yes | integer(int32) |  |

#### Request Body

- Required: No

| Content-Type | Schema |
|---|---|
| application/json | ContractFileRequest |
| text/json | ContractFileRequest |
| application/*+json | ContractFileRequest |

Sample JSON request:

```json
{
    "name":  "sample",
    "fileUrl":  "sample",
    "contractTypeId":  1,
    "note":  "sample"
}
```

#### Responses

| Status | Description | Content-Type | Schema |
|---|---|---|---|
| 200 | Success |  |  |

### DELETE /api/ContractFiles/{id}

- OperationId: 
- Auth (Swagger): No
- Auth (Observed): Yes

#### Parameters

| Name | In | Required | Type | Description |
|---|---|---|---|---|
| id | path | Yes | integer(int32) |  |

#### Responses

| Status | Description | Content-Type | Schema |
|---|---|---|---|
| 200 | Success |  |  |

## Tag: ContractTypes

### GET /api/ContractTypes

- OperationId: 
- Auth (Swagger): No
- Auth (Observed): Yes

#### Responses

| Status | Description | Content-Type | Schema |
|---|---|---|---|
| 200 | Success |  |  |

### POST /api/ContractTypes

- OperationId: 
- Auth (Swagger): No
- Auth (Observed): Yes

#### Request Body

- Required: No

| Content-Type | Schema |
|---|---|
| application/json | ContractTypeRequest |
| text/json | ContractTypeRequest |
| application/*+json | ContractTypeRequest |

Sample JSON request:

```json
{
    "name":  "sample",
    "code":  "sample",
    "description":  "sample",
    "durationMonths":  1
}
```

#### Responses

| Status | Description | Content-Type | Schema |
|---|---|---|---|
| 200 | Success |  |  |

### GET /api/ContractTypes/{id}

- OperationId: 
- Auth (Swagger): No
- Auth (Observed): Yes

#### Parameters

| Name | In | Required | Type | Description |
|---|---|---|---|---|
| id | path | Yes | integer(int32) |  |

#### Responses

| Status | Description | Content-Type | Schema |
|---|---|---|---|
| 200 | Success |  |  |

### PUT /api/ContractTypes/{id}

- OperationId: 
- Auth (Swagger): No
- Auth (Observed): Yes

#### Parameters

| Name | In | Required | Type | Description |
|---|---|---|---|---|
| id | path | Yes | integer(int32) |  |

#### Request Body

- Required: No

| Content-Type | Schema |
|---|---|
| application/json | ContractTypeRequest |
| text/json | ContractTypeRequest |
| application/*+json | ContractTypeRequest |

Sample JSON request:

```json
{
    "name":  "sample",
    "code":  "sample",
    "description":  "sample",
    "durationMonths":  1
}
```

#### Responses

| Status | Description | Content-Type | Schema |
|---|---|---|---|
| 200 | Success |  |  |

### DELETE /api/ContractTypes/{id}

- OperationId: 
- Auth (Swagger): No
- Auth (Observed): Yes

#### Parameters

| Name | In | Required | Type | Description |
|---|---|---|---|---|
| id | path | Yes | integer(int32) |  |

#### Responses

| Status | Description | Content-Type | Schema |
|---|---|---|---|
| 200 | Success |  |  |

## Tag: Decides

### GET /api/Decides

- OperationId: 
- Auth (Swagger): No
- Auth (Observed): Yes

#### Responses

| Status | Description | Content-Type | Schema |
|---|---|---|---|
| 200 | Success |  |  |

### POST /api/Decides

- OperationId: 
- Auth (Swagger): No
- Auth (Observed): Yes

#### Request Body

- Required: No

| Content-Type | Schema |
|---|---|
| application/json | DecideRequest |
| text/json | DecideRequest |
| application/*+json | DecideRequest |

Sample JSON request:

```json
{
    "userId":  1,
    "decisionTypeId":  1,
    "title":  "sample",
    "description":  "sample",
    "effectiveDate":  "2026-03-13T11:57:26.9654610+07:00",
    "expiredDate":  "2026-03-13T11:57:26.9654610+07:00",
    "fileUrl":  "sample",
    "note":  "sample"
}
```

#### Responses

| Status | Description | Content-Type | Schema |
|---|---|---|---|
| 200 | Success |  |  |

### GET /api/Decides/{id}

- OperationId: 
- Auth (Swagger): No
- Auth (Observed): Yes

#### Parameters

| Name | In | Required | Type | Description |
|---|---|---|---|---|
| id | path | Yes | integer(int32) |  |

#### Responses

| Status | Description | Content-Type | Schema |
|---|---|---|---|
| 200 | Success |  |  |

### PUT /api/Decides/{id}

- OperationId: 
- Auth (Swagger): No
- Auth (Observed): Yes

#### Parameters

| Name | In | Required | Type | Description |
|---|---|---|---|---|
| id | path | Yes | integer(int32) |  |

#### Request Body

- Required: No

| Content-Type | Schema |
|---|---|
| application/json | DecideRequest |
| text/json | DecideRequest |
| application/*+json | DecideRequest |

Sample JSON request:

```json
{
    "userId":  1,
    "decisionTypeId":  1,
    "title":  "sample",
    "description":  "sample",
    "effectiveDate":  "2026-03-13T11:57:26.9686746+07:00",
    "expiredDate":  "2026-03-13T11:57:26.9686746+07:00",
    "fileUrl":  "sample",
    "note":  "sample"
}
```

#### Responses

| Status | Description | Content-Type | Schema |
|---|---|---|---|
| 200 | Success |  |  |

### DELETE /api/Decides/{id}

- OperationId: 
- Auth (Swagger): No
- Auth (Observed): Yes

#### Parameters

| Name | In | Required | Type | Description |
|---|---|---|---|---|
| id | path | Yes | integer(int32) |  |

#### Responses

| Status | Description | Content-Type | Schema |
|---|---|---|---|
| 200 | Success |  |  |

### GET /api/Decides/user/{userId}

- OperationId: 
- Auth (Swagger): No
- Auth (Observed): Yes

#### Parameters

| Name | In | Required | Type | Description |
|---|---|---|---|---|
| userId | path | Yes | integer(int32) |  |

#### Responses

| Status | Description | Content-Type | Schema |
|---|---|---|---|
| 200 | Success |  |  |

## Tag: DecisionTypes

### GET /api/DecisionTypes

- OperationId: 
- Auth (Swagger): No
- Auth (Observed): Yes

#### Responses

| Status | Description | Content-Type | Schema |
|---|---|---|---|
| 200 | Success |  |  |

### POST /api/DecisionTypes

- OperationId: 
- Auth (Swagger): No
- Auth (Observed): Yes

#### Request Body

- Required: No

| Content-Type | Schema |
|---|---|
| application/json | DecisionTypeRequest |
| text/json | DecisionTypeRequest |
| application/*+json | DecisionTypeRequest |

Sample JSON request:

```json
{
    "name":  "sample",
    "code":  "sample",
    "note":  "sample"
}
```

#### Responses

| Status | Description | Content-Type | Schema |
|---|---|---|---|
| 200 | Success |  |  |

### GET /api/DecisionTypes/{id}

- OperationId: 
- Auth (Swagger): No
- Auth (Observed): Yes

#### Parameters

| Name | In | Required | Type | Description |
|---|---|---|---|---|
| id | path | Yes | integer(int32) |  |

#### Responses

| Status | Description | Content-Type | Schema |
|---|---|---|---|
| 200 | Success |  |  |

### PUT /api/DecisionTypes/{id}

- OperationId: 
- Auth (Swagger): No
- Auth (Observed): Yes

#### Parameters

| Name | In | Required | Type | Description |
|---|---|---|---|---|
| id | path | Yes | integer(int32) |  |

#### Request Body

- Required: No

| Content-Type | Schema |
|---|---|
| application/json | DecisionTypeRequest |
| text/json | DecisionTypeRequest |
| application/*+json | DecisionTypeRequest |

Sample JSON request:

```json
{
    "name":  "sample",
    "code":  "sample",
    "note":  "sample"
}
```

#### Responses

| Status | Description | Content-Type | Schema |
|---|---|---|---|
| 200 | Success |  |  |

### DELETE /api/DecisionTypes/{id}

- OperationId: 
- Auth (Swagger): No
- Auth (Observed): Yes

#### Parameters

| Name | In | Required | Type | Description |
|---|---|---|---|---|
| id | path | Yes | integer(int32) |  |

#### Responses

| Status | Description | Content-Type | Schema |
|---|---|---|---|
| 200 | Success |  |  |

## Tag: Degrees

### GET /api/Degrees

- OperationId: 
- Auth (Swagger): No
- Auth (Observed): Yes

#### Responses

| Status | Description | Content-Type | Schema |
|---|---|---|---|
| 200 | Success |  |  |

### POST /api/Degrees

- OperationId: 
- Auth (Swagger): No
- Auth (Observed): Yes

#### Request Body

- Required: No

| Content-Type | Schema |
|---|---|
| application/json | DegreeRequest |
| text/json | DegreeRequest |
| application/*+json | DegreeRequest |

Sample JSON request:

```json
{
    "userId":  1,
    "name":  "sample",
    "school":  "sample",
    "description":  "sample",
    "graduationYear":  1
}
```

#### Responses

| Status | Description | Content-Type | Schema |
|---|---|---|---|
| 200 | Success |  |  |

### GET /api/Degrees/{id}

- OperationId: 
- Auth (Swagger): No
- Auth (Observed): Yes

#### Parameters

| Name | In | Required | Type | Description |
|---|---|---|---|---|
| id | path | Yes | integer(int32) |  |

#### Responses

| Status | Description | Content-Type | Schema |
|---|---|---|---|
| 200 | Success |  |  |

### PUT /api/Degrees/{id}

- OperationId: 
- Auth (Swagger): No
- Auth (Observed): Yes

#### Parameters

| Name | In | Required | Type | Description |
|---|---|---|---|---|
| id | path | Yes | integer(int32) |  |

#### Request Body

- Required: No

| Content-Type | Schema |
|---|---|
| application/json | DegreeRequest |
| text/json | DegreeRequest |
| application/*+json | DegreeRequest |

Sample JSON request:

```json
{
    "userId":  1,
    "name":  "sample",
    "school":  "sample",
    "description":  "sample",
    "graduationYear":  1
}
```

#### Responses

| Status | Description | Content-Type | Schema |
|---|---|---|---|
| 200 | Success |  |  |

### DELETE /api/Degrees/{id}

- OperationId: 
- Auth (Swagger): No
- Auth (Observed): Yes

#### Parameters

| Name | In | Required | Type | Description |
|---|---|---|---|---|
| id | path | Yes | integer(int32) |  |

#### Responses

| Status | Description | Content-Type | Schema |
|---|---|---|---|
| 200 | Success |  |  |

### GET /api/Degrees/user/{userId}

- OperationId: 
- Auth (Swagger): No
- Auth (Observed): Yes

#### Parameters

| Name | In | Required | Type | Description |
|---|---|---|---|---|
| userId | path | Yes | integer(int32) |  |

#### Responses

| Status | Description | Content-Type | Schema |
|---|---|---|---|
| 200 | Success |  |  |

## Tag: Departments

### GET /api/Departments

- OperationId: 
- Auth (Swagger): No
- Auth (Observed): Yes

#### Responses

| Status | Description | Content-Type | Schema |
|---|---|---|---|
| 200 | Success |  |  |

### POST /api/Departments

- OperationId: 
- Auth (Swagger): No
- Auth (Observed): Yes

#### Request Body

- Required: No

| Content-Type | Schema |
|---|---|
| application/json | DepartmentRequest |
| text/json | DepartmentRequest |
| application/*+json | DepartmentRequest |

Sample JSON request:

```json
{
    "name":  "sample",
    "code":  "sample",
    "parentId":  1,
    "order":  1
}
```

#### Responses

| Status | Description | Content-Type | Schema |
|---|---|---|---|
| 200 | Success |  |  |

### GET /api/Departments/{id}

- OperationId: 
- Auth (Swagger): No
- Auth (Observed): Yes

#### Parameters

| Name | In | Required | Type | Description |
|---|---|---|---|---|
| id | path | Yes | integer(int32) |  |

#### Responses

| Status | Description | Content-Type | Schema |
|---|---|---|---|
| 200 | Success |  |  |

### PUT /api/Departments/{id}

- OperationId: 
- Auth (Swagger): No
- Auth (Observed): Yes

#### Parameters

| Name | In | Required | Type | Description |
|---|---|---|---|---|
| id | path | Yes | integer(int32) |  |

#### Request Body

- Required: No

| Content-Type | Schema |
|---|---|
| application/json | DepartmentRequest |
| text/json | DepartmentRequest |
| application/*+json | DepartmentRequest |

Sample JSON request:

```json
{
    "name":  "sample",
    "code":  "sample",
    "parentId":  1,
    "order":  1
}
```

#### Responses

| Status | Description | Content-Type | Schema |
|---|---|---|---|
| 200 | Success |  |  |

### DELETE /api/Departments/{id}

- OperationId: 
- Auth (Swagger): No
- Auth (Observed): Yes

#### Parameters

| Name | In | Required | Type | Description |
|---|---|---|---|---|
| id | path | Yes | integer(int32) |  |

#### Responses

| Status | Description | Content-Type | Schema |
|---|---|---|---|
| 200 | Success |  |  |

## Tag: DriverRouters

### GET /api/DriverRouters

- OperationId: 
- Auth (Swagger): No
- Auth (Observed): Yes

#### Parameters

| Name | In | Required | Type | Description |
|---|---|---|---|---|
| Page | query | No | integer(int32) |  |
| PageSize | query | No | integer(int32) |  |
| SearchText | query | No | string |  |

#### Responses

| Status | Description | Content-Type | Schema |
|---|---|---|---|
| 200 | Success |  |  |

### GET /api/DriverRouters/{id}

- OperationId: 
- Auth (Swagger): No
- Auth (Observed): Yes

#### Parameters

| Name | In | Required | Type | Description |
|---|---|---|---|---|
| id | path | Yes | integer(int32) |  |

#### Responses

| Status | Description | Content-Type | Schema |
|---|---|---|---|
| 200 | Success |  |  |

### PUT /api/DriverRouters/{id}

- OperationId: 
- Auth (Swagger): No
- Auth (Observed): Yes

#### Parameters

| Name | In | Required | Type | Description |
|---|---|---|---|---|
| id | path | Yes | integer(int32) |  |

#### Request Body

- Required: No

| Content-Type | Schema |
|---|---|
| application/json | DriverRouterModel |
| text/json | DriverRouterModel |
| application/*+json | DriverRouterModel |

Sample JSON request:

```json
{
    "id":  1,
    "date":  "2026-03-13T11:57:27.0097099+07:00",
    "amount":  1,
    "note":  "sample",
    "status":  "sample",
    "petrolConsumptionId":  1,
    "advancePaymentAmount":  1,
    "fuelAmount":  1
}
```

#### Responses

| Status | Description | Content-Type | Schema |
|---|---|---|---|
| 200 | Success |  |  |

### DELETE /api/DriverRouters/{id}

- OperationId: 
- Auth (Swagger): No
- Auth (Observed): Yes

#### Parameters

| Name | In | Required | Type | Description |
|---|---|---|---|---|
| id | path | Yes | integer(int32) |  |

#### Responses

| Status | Description | Content-Type | Schema |
|---|---|---|---|
| 200 | Success |  |  |

### POST /api/DriverRouters/finish

- OperationId: 
- Auth (Swagger): No
- Auth (Observed): Yes

#### Parameters

| Name | In | Required | Type | Description |
|---|---|---|---|---|
| petrolConsumptionId | query | No | integer(int32) |  |

#### Responses

| Status | Description | Content-Type | Schema |
|---|---|---|---|
| 200 | Success |  |  |

### GET /api/DriverRouters/list/police-point/{id}

- OperationId: 
- Auth (Swagger): No
- Auth (Observed): Yes

#### Parameters

| Name | In | Required | Type | Description |
|---|---|---|---|---|
| id | path | Yes | integer(int32) |  |

#### Responses

| Status | Description | Content-Type | Schema |
|---|---|---|---|
| 200 | Success |  |  |

### POST /api/DriverRouters/start

- OperationId: 
- Auth (Swagger): No
- Auth (Observed): Yes

#### Parameters

| Name | In | Required | Type | Description |
|---|---|---|---|---|
| petrolConsumptionId | query | No | integer(int32) |  |

#### Responses

| Status | Description | Content-Type | Schema |
|---|---|---|---|
| 200 | Success |  |  |

## Tag: Facebook

### GET /api/v1/multichannel/app-config

- OperationId: 
- Auth (Swagger): No
- Auth (Observed): No

#### Responses

| Status | Description | Content-Type | Schema |
|---|---|---|---|
| 200 | Success |  |  |

### POST /api/v1/multichannel/app-config

- OperationId: 
- Auth (Swagger): No
- Auth (Observed): No

#### Request Body

- Required: No

| Content-Type | Schema |
|---|---|
| application/json | unknown |
| text/json | unknown |
| application/*+json | unknown |

Sample JSON request:

```json
"sample"
```

#### Responses

| Status | Description | Content-Type | Schema |
|---|---|---|---|
| 200 | Success |  |  |

### GET /api/v1/multichannel/automation

- OperationId: 
- Auth (Swagger): No
- Auth (Observed): No

#### Responses

| Status | Description | Content-Type | Schema |
|---|---|---|---|
| 200 | Success |  |  |

### POST /api/v1/multichannel/automation

- OperationId: 
- Auth (Swagger): No
- Auth (Observed): No

#### Request Body

- Required: No

| Content-Type | Schema |
|---|---|
| application/json | WorkflowDto |
| text/json | WorkflowDto |
| application/*+json | WorkflowDto |

Sample JSON request:

```json
{
    "name":  "sample",
    "workflowJson":  "sample",
    "triggerType":  "sample"
}
```

---

## Frontend Integration Guide (Customer + Sell Compatibility APIs)

Updated: 2026-04-04

This section is frontend-focused and documents the compatibility APIs that were added from legacy module patterns (ModuleCustomer + ModuleSell) into AciPlatform.

### Environment

- Base URL (local test): `http://localhost:5041`
- Auth: Bearer JWT
- Content type: `application/json`

### Smoke Test Status (2026-04-04)

- `GET /swagger/v1/swagger.json`: **200**
- `POST /api/WebAuth/login` with invalid credentials: **401**
- `GET /api/Customers` without token: **401**
- `GET /api/Goods` without token: **401**
- `GET /api/Order` without token: **401**
- Authenticated flow (register -> login -> call protected APIs): verified successfully via server execution logs and DB query traces.

## 1) Authentication Flow for Frontend

### 1.1 Register test/customer account

- Endpoint: `POST /api/WebAuth/register`
- Auth: No

Request body example:

```json
{
    "name": "Frontend Test User",
    "phone": "0912345678",
    "password": "Test@123",
    "email": "frontend.test@example.com",
    "address": "HCM"
}
```

Response body example:

```json
{
    "status": 200,
    "data": {
        "id": 123,
        "code": "CUS20260001",
        "name": "Frontend Test User",
        "avatar": null,
        "phone": "0912345678",
        "token": "<jwt-token>"
    }
}
```

### 1.2 Login

- Endpoint: `POST /api/WebAuth/login`
- Auth: No

Request body example:

```json
{
    "username": "0912345678",
    "password": "Test@123"
}
```

Success response includes token at `Token`.

```json
{
    "id": 123,
    "username": "CUS20260001",
    "fullname": "Frontend Test User",
    "token": "<jwt-token>",
    "email": "frontend.test@example.com",
    "phone": "0912345678"
}
```

Use header for protected APIs:

`Authorization: Bearer <jwt-token>`

---

## 2) Customers API (Compatibility)

Controller route: `/api/Customers`

### 2.1 Get paging list

- `GET /api/Customers?page=1&pageSize=20&searchText=&code=&phone=&email=`
- Auth: Yes

Response shape:

```json
{
    "currentPage": 1,
    "pageSize": 20,
    "totalItems": 100,
    "data": [
        {
            "id": 1,
            "code": "CUS20260001",
            "name": "Nguyen Van A",
            "phone": "0912345678",
            "email": "a@example.com",
            "address": "HCM",
            "createdDate": "2026-04-04T08:00:00"
        }
    ]
}
```

### 2.2 Get select list

- `GET /api/Customers/list?searchText=nguyen`
- Auth: Yes

### 2.3 Get code-name list

- `GET /api/Customers/list-code-name`
- Auth: Yes

### 2.4 Get customer detail

- `GET /api/Customers/{id}`
- Auth: Yes

### 2.5 Create customer

- `POST /api/Customers`
- Auth: Yes

Request body example:

```json
{
    "code": "",
    "name": "Le Thi B",
    "phone": "0988111222",
    "email": "b@example.com",
    "address": "Ha Noi",
    "provinceId": 1,
    "districtId": 2,
    "wardId": 3,
    "gender": 1
}
```

Notes:

- If `code` is empty, backend auto-generates format `CUS{yyyy}{0001}`.
- Phone is unique among non-deleted customers.

### 2.6 Update customer

- `PUT /api/Customers/{id}`
- Auth: Yes

### 2.7 Delete customer (soft delete)

- `DELETE /api/Customers/{id}`
- Auth: Yes

### 2.8 Get generated customer code

- `GET /api/Customers/get-code-customer`
- Auth: Yes

### 2.9 Get customer warnings

- `GET /api/Customers/get-customer-warning`
- Auth: Yes
- Current warning rule: customer missing email.

---

## 3) Goods API (Compatibility)

Controller route: `/api/Goods`

Source data mapping: `GoodWarehouses` (QLKho)

### 3.1 Get paging goods

- `GET /api/Goods?page=1&pageSize=20&searchText=&goodType=&account=&detail1=&priceCode=&menuType=&warehouse=&goodCode=&status=0`
- Auth: Yes

Response shape:

```json
{
    "data": [
        {
            "id": 1,
            "menuType": "FOOD",
            "account": "ACC01",
            "accountName": "Account Name",
            "warehouse": "WH01",
            "warehouseName": "Main Warehouse",
            "detail1": "SP001",
            "detail2": "SP001A",
            "detailName1": "San pham 1",
            "detailName2": "San pham 1 - Bien the",
            "goodsType": "NORMAL",
            "quantity": 120,
            "status": 1,
            "goodCode": "SP001A",
            "goodName": "San pham 1 - Bien the",
            "qrCode": "SP001A 1-1"
        }
    ],
    "totalItems": 1200,
    "pageSize": 20,
    "currentPage": 1
}
```

### 3.2 Get simple goods list

- `GET /api/Goods/list`
- Auth: Yes

### 3.3 Get goods detail by id

- `GET /api/Goods/{id}`
- Auth: Yes

### 3.4 Goods report endpoint

- `GET /api/Goods/report-good-in-warehouse` (same filter as 3.1)
- Auth: Yes
- Behavior: returns same data shape as paging goods.

### 3.5 Sync account endpoint (compatibility)

- `GET /api/Goods/SyncAccountGood`
- Auth: Yes
- Current behavior: accepts request and returns success payload for frontend flow compatibility.

---

## 4) Order API (Compatibility)

Controller route: `/api/Order`

Source data mapping: `GoodWarehouseExports` grouped by `BillId`

### 4.1 Search orders

- `GET /api/Order?page=1&pageSize=20&searchText=&billId=&fromDate=&toDate=`
- Auth: Yes

Response shape:

```json
{
    "data": [
        {
            "billId": 12345,
            "totalLineItems": 3,
            "createdAt": "2026-04-03T09:30:00",
            "isDeleted": false
        }
    ],
    "totalItems": 50,
    "pageSize": 20,
    "currentPage": 1
}
```

### 4.2 Get order detail

- `GET /api/Order/{id}` where `id = BillId`
- Auth: Yes

Returns list line items:

```json
[
    {
        "exportId": 1,
        "goodWarehouseId": 100,
        "goodCode": "SP001A",
        "goodName": "San pham 1 - Bien the",
        "quantity": 5,
        "createdAt": "2026-04-03T09:30:00"
    }
]
```

### 4.3 Update order soft-delete state

- `PUT /api/Order/{id}`
- Auth: Yes

Request body example:

```json
{
    "billId": 12345,
    "isDeleted": true
}
```

### 4.4 Notification orders

- `GET /api/Order/notification-order`
- Auth: Yes
- Returns top newest bill groups created today.

---

## 5) Frontend Error Handling Contract

### Common status codes

- `200`: success
- `400`: bad request / validation error
- `401`: missing or invalid token
- `404`: resource not found

### Suggested axios/fetch interceptor behavior

- If `401`: redirect login + clear local token.
- If `400`: display backend `msg` or `message` when available.
- If `404`: show empty state or not-found page depending on screen.

---

## 6) Quick Frontend Checklist

1. Login/Register and persist JWT token.
2. Attach `Authorization: Bearer <token>` on all protected APIs.
3. Use paging params (`page`, `pageSize`) consistently.
4. For orders, treat route id as `BillId` (not export row id).
5. For customers, avoid duplicate phone when creating/updating.


#### Responses

| Status | Description | Content-Type | Schema |
|---|---|---|---|
| 200 | Success |  |  |

### POST /api/v1/multichannel/connect-page

- OperationId: 
- Auth (Swagger): No
- Auth (Observed): No

#### Request Body

- Required: No

| Content-Type | Schema |
|---|---|
| application/json | ConnectPageDto |
| text/json | ConnectPageDto |
| application/*+json | ConnectPageDto |

Sample JSON request:

```json
{
    "pageId":  "sample",
    "name":  "sample",
    "accessToken":  "sample",
    "userToken":  "sample"
}
```

#### Responses

| Status | Description | Content-Type | Schema |
|---|---|---|---|
| 200 | Success |  |  |

### POST /api/v1/multichannel/disconnect-page/{pageId}

- OperationId: 
- Auth (Swagger): No
- Auth (Observed): No

#### Parameters

| Name | In | Required | Type | Description |
|---|---|---|---|---|
| pageId | path | Yes | integer(int32) |  |

#### Responses

| Status | Description | Content-Type | Schema |
|---|---|---|---|
| 200 | Success |  |  |

### GET /api/v1/multichannel/insights/{pageId}

- OperationId: 
- Auth (Swagger): No
- Auth (Observed): No

#### Parameters

| Name | In | Required | Type | Description |
|---|---|---|---|---|
| pageId | path | Yes | integer(int32) |  |
| metric | query | No | string |  |

#### Responses

| Status | Description | Content-Type | Schema |
|---|---|---|---|
| 200 | Success |  |  |

### POST /api/v1/multichannel/oauth-callback

- OperationId: 
- Auth (Swagger): No
- Auth (Observed): No

#### Request Body

- Required: No

| Content-Type | Schema |
|---|---|
| application/json | unknown |
| text/json | unknown |
| application/*+json | unknown |

Sample JSON request:

```json
"sample"
```

#### Responses

| Status | Description | Content-Type | Schema |
|---|---|---|---|
| 200 | Success |  |  |

### GET /api/v1/multichannel/oauth-url

- OperationId: 
- Auth (Swagger): No
- Auth (Observed): No

#### Parameters

| Name | In | Required | Type | Description |
|---|---|---|---|---|
| redirectUri | query | No | string |  |

#### Responses

| Status | Description | Content-Type | Schema |
|---|---|---|---|
| 200 | Success |  |  |

### GET /api/v1/multichannel/pages

- OperationId: 
- Auth (Swagger): No
- Auth (Observed): No

#### Responses

| Status | Description | Content-Type | Schema |
|---|---|---|---|
| 200 | Success |  |  |

### POST /api/v1/multichannel/post

- OperationId: 
- Auth (Swagger): No
- Auth (Observed): No

#### Request Body

- Required: No

| Content-Type | Schema |
|---|---|
| application/json | CreatePostDto |
| text/json | CreatePostDto |
| application/*+json | CreatePostDto |

Sample JSON request:

```json
{
    "pageId":  1,
    "content":  "sample",
    "imageUrls":  "sample",
    "scheduledTime":  "2026-03-13T11:57:27.0400064+07:00",
    "autoGenerateContent":  true,
    "aiPrompt":  "sample"
}
```

#### Responses

| Status | Description | Content-Type | Schema |
|---|---|---|---|
| 200 | Success |  |  |

### POST /api/v1/multichannel/publish

- OperationId: 
- Auth (Swagger): No
- Auth (Observed): No

#### Request Body

- Required: No

| Content-Type | Schema |
|---|---|
| application/json | unknown |
| text/json | unknown |
| application/*+json | unknown |

Sample JSON request:

```json
"sample"
```

#### Responses

| Status | Description | Content-Type | Schema |
|---|---|---|---|
| 200 | Success |  |  |

## Tag: GoodWarehouseExports

### GET /api/GoodWarehouseExports

- OperationId: 
- Auth (Swagger): No
- Auth (Observed): Yes

#### Parameters

| Name | In | Required | Type | Description |
|---|---|---|---|---|
| Fromdt | query | No | string(date-time) |  |
| Page | query | No | integer(int32) |  |
| PageSize | query | No | integer(int32) |  |
| SearchText | query | No | string |  |
| Todt | query | No | string(date-time) |  |

#### Responses

| Status | Description | Content-Type | Schema |
|---|---|---|---|
| 200 | Success |  |  |

### DELETE /api/GoodWarehouseExports/{billId}

- OperationId: 
- Auth (Swagger): No
- Auth (Observed): Yes

#### Parameters

| Name | In | Required | Type | Description |
|---|---|---|---|---|
| billId | path | Yes | integer(int32) |  |

#### Responses

| Status | Description | Content-Type | Schema |
|---|---|---|---|
| 200 | Success |  |  |

## Tag: GoodWarehouses

### GET /api/GoodWarehouses

- OperationId: 
- Auth (Swagger): No
- Auth (Observed): Yes

#### Parameters

| Name | In | Required | Type | Description |
|---|---|---|---|---|
| Account | query | No | string |  |
| Detail1 | query | No | string |  |
| GoodCode | query | No | string |  |
| GoodType | query | No | string |  |
| MenuType | query | No | string |  |
| Page | query | No | integer(int32) |  |
| PageSize | query | No | integer(int32) |  |
| PriceCode | query | No | string |  |
| SearchText | query | No | string |  |
| Status | query | No | integer(int32) |  |
| Warehouse | query | No | string |  |

#### Responses

| Status | Description | Content-Type | Schema |
|---|---|---|---|
| 200 | Success |  |  |

### POST /api/GoodWarehouses/sync

- OperationId: 
- Auth (Swagger): No
- Auth (Observed): Yes

#### Parameters

| Name | In | Required | Type | Description |
|---|---|---|---|---|
| yearFilter | header | No | integer(int32) |  |

#### Responses

| Status | Description | Content-Type | Schema |
|---|---|---|---|
| 200 | Success |  |  |

## Tag: HistoryAchievements

### GET /api/HistoryAchievements

- OperationId: 
- Auth (Swagger): No
- Auth (Observed): Yes

#### Responses

| Status | Description | Content-Type | Schema |
|---|---|---|---|
| 200 | Success |  |  |

### POST /api/HistoryAchievements

- OperationId: 
- Auth (Swagger): No
- Auth (Observed): Yes

#### Request Body

- Required: No

| Content-Type | Schema |
|---|---|
| application/json | HistoryAchievementRequest |
| text/json | HistoryAchievementRequest |
| application/*+json | HistoryAchievementRequest |

Sample JSON request:

```json
{
    "userId":  1,
    "title":  "sample",
    "description":  "sample",
    "achievedDate":  "2026-03-13T11:57:27.0552432+07:00",
    "note":  "sample"
}
```

#### Responses

| Status | Description | Content-Type | Schema |
|---|---|---|---|
| 200 | Success |  |  |

### GET /api/HistoryAchievements/{id}

- OperationId: 
- Auth (Swagger): No
- Auth (Observed): Yes

#### Parameters

| Name | In | Required | Type | Description |
|---|---|---|---|---|
| id | path | Yes | integer(int32) |  |

#### Responses

| Status | Description | Content-Type | Schema |
|---|---|---|---|
| 200 | Success |  |  |

### PUT /api/HistoryAchievements/{id}

- OperationId: 
- Auth (Swagger): No
- Auth (Observed): Yes

#### Parameters

| Name | In | Required | Type | Description |
|---|---|---|---|---|
| id | path | Yes | integer(int32) |  |

#### Request Body

- Required: No

| Content-Type | Schema |
|---|---|
| application/json | HistoryAchievementRequest |
| text/json | HistoryAchievementRequest |
| application/*+json | HistoryAchievementRequest |

Sample JSON request:

```json
{
    "userId":  1,
    "title":  "sample",
    "description":  "sample",
    "achievedDate":  "2026-03-13T11:57:27.0625385+07:00",
    "note":  "sample"
}
```

#### Responses

| Status | Description | Content-Type | Schema |
|---|---|---|---|
| 200 | Success |  |  |

### DELETE /api/HistoryAchievements/{id}

- OperationId: 
- Auth (Swagger): No
- Auth (Observed): Yes

#### Parameters

| Name | In | Required | Type | Description |
|---|---|---|---|---|
| id | path | Yes | integer(int32) |  |

#### Responses

| Status | Description | Content-Type | Schema |
|---|---|---|---|
| 200 | Success |  |  |

### GET /api/HistoryAchievements/user/{userId}

- OperationId: 
- Auth (Swagger): No
- Auth (Observed): Yes

#### Parameters

| Name | In | Required | Type | Description |
|---|---|---|---|---|
| userId | path | Yes | integer(int32) |  |

#### Responses

| Status | Description | Content-Type | Schema |
|---|---|---|---|
| 200 | Success |  |  |

## Tag: Inventories

### GET /api/Inventories

- OperationId: 
- Auth (Swagger): No
- Auth (Observed): Yes

#### Parameters

| Name | In | Required | Type | Description |
|---|---|---|---|---|
| yearFilter | header | No | integer(int32) |  |
| Page | query | No | integer(int32) |  |
| PageSize | query | No | integer(int32) |  |
| SearchText | query | No | string |  |

#### Responses

| Status | Description | Content-Type | Schema |
|---|---|---|---|
| 200 | Success |  |  |

### POST /api/Inventories

- OperationId: 
- Auth (Swagger): No
- Auth (Observed): Yes

#### Request Body

- Required: No

| Content-Type | Schema |
|---|---|
| application/json | array<Inventory> |
| text/json | array<Inventory> |
| application/*+json | array<Inventory> |

Sample JSON request:

```json
{
    "id":  1,
    "account":  "sample",
    "accountName":  "sample",
    "warehouse":  "sample",
    "warehouseName":  "sample",
    "detail1":  "sample",
    "detailName1":  "sample",
    "detail2":  "sample",
    "detailName2":  "sample",
    "image1":  "sample",
    "inputQuantity":  1,
    "outputQuantity":  1,
    "closeQuantity":  1,
    "closeQuantityReal":  1,
    "createAt":  "2026-03-13T11:57:27.0727553+07:00",
    "note":  "sample",
    "dateExpiration":  "2026-03-13T11:57:27.0727553+07:00",
    "isCheck":  true,
    "isDeleted":  true
}
```

#### Responses

| Status | Description | Content-Type | Schema |
|---|---|---|---|
| 200 | Success |  |  |

### POST /api/Inventories/accept

- OperationId: 
- Auth (Swagger): No
- Auth (Observed): Yes

#### Request Body

- Required: No

| Content-Type | Schema |
|---|---|
| application/json | array<Inventory> |
| text/json | array<Inventory> |
| application/*+json | array<Inventory> |

Sample JSON request:

```json
{
    "id":  1,
    "account":  "sample",
    "accountName":  "sample",
    "warehouse":  "sample",
    "warehouseName":  "sample",
    "detail1":  "sample",
    "detailName1":  "sample",
    "detail2":  "sample",
    "detailName2":  "sample",
    "image1":  "sample",
    "inputQuantity":  1,
    "outputQuantity":  1,
    "closeQuantity":  1,
    "closeQuantityReal":  1,
    "createAt":  "2026-03-13T11:57:27.0727553+07:00",
    "note":  "sample",
    "dateExpiration":  "2026-03-13T11:57:27.0727553+07:00",
    "isCheck":  true,
    "isDeleted":  true
}
```

#### Responses

| Status | Description | Content-Type | Schema |
|---|---|---|---|
| 200 | Success |  |  |

### GET /api/Inventories/list-date

- OperationId: 
- Auth (Swagger): No
- Auth (Observed): Yes

#### Responses

| Status | Description | Content-Type | Schema |
|---|---|---|---|
| 200 | Success |  |  |

### GET /api/Inventories/list-inventory

- OperationId: 
- Auth (Swagger): No
- Auth (Observed): Yes

#### Parameters

| Name | In | Required | Type | Description |
|---|---|---|---|---|
| dtMax | query | No | string(date-time) |  |

#### Responses

| Status | Description | Content-Type | Schema |
|---|---|---|---|
| 200 | Success |  |  |

## Tag: Majors

### GET /api/Majors

- OperationId: 
- Auth (Swagger): No
- Auth (Observed): Yes

#### Responses

| Status | Description | Content-Type | Schema |
|---|---|---|---|
| 200 | Success |  |  |

### POST /api/Majors

- OperationId: 
- Auth (Swagger): No
- Auth (Observed): Yes

#### Request Body

- Required: No

| Content-Type | Schema |
|---|---|
| application/json | MajorRequest |
| text/json | MajorRequest |
| application/*+json | MajorRequest |

Sample JSON request:

```json
{
    "name":  "sample",
    "description":  "sample"
}
```

#### Responses

| Status | Description | Content-Type | Schema |
|---|---|---|---|
| 200 | Success |  |  |

### GET /api/Majors/{id}

- OperationId: 
- Auth (Swagger): No
- Auth (Observed): Yes

#### Parameters

| Name | In | Required | Type | Description |
|---|---|---|---|---|
| id | path | Yes | integer(int32) |  |

#### Responses

| Status | Description | Content-Type | Schema |
|---|---|---|---|
| 200 | Success |  |  |

### PUT /api/Majors/{id}

- OperationId: 
- Auth (Swagger): No
- Auth (Observed): Yes

#### Parameters

| Name | In | Required | Type | Description |
|---|---|---|---|---|
| id | path | Yes | integer(int32) |  |

#### Request Body

- Required: No

| Content-Type | Schema |
|---|---|
| application/json | MajorRequest |
| text/json | MajorRequest |
| application/*+json | MajorRequest |

Sample JSON request:

```json
{
    "name":  "sample",
    "description":  "sample"
}
```

#### Responses

| Status | Description | Content-Type | Schema |
|---|---|---|---|
| 200 | Success |  |  |

### DELETE /api/Majors/{id}

- OperationId: 
- Auth (Swagger): No
- Auth (Observed): Yes

#### Parameters

| Name | In | Required | Type | Description |
|---|---|---|---|---|
| id | path | Yes | integer(int32) |  |

#### Responses

| Status | Description | Content-Type | Schema |
|---|---|---|---|
| 200 | Success |  |  |

## Tag: MenuRoles

### GET /api/MenuRoles/role/{roleId}

- OperationId: 
- Auth (Swagger): No
- Auth (Observed): Yes

#### Parameters

| Name | In | Required | Type | Description |
|---|---|---|---|---|
| roleId | path | Yes | integer(int32) |  |

#### Responses

| Status | Description | Content-Type | Schema |
|---|---|---|---|
| 200 | Success |  |  |

### POST /api/MenuRoles/role/{roleId}

- OperationId: 
- Auth (Swagger): No
- Auth (Observed): Yes

#### Parameters

| Name | In | Required | Type | Description |
|---|---|---|---|---|
| roleId | path | Yes | integer(int32) |  |

#### Request Body

- Required: No

| Content-Type | Schema |
|---|---|
| application/json | array<MenuRole> |
| text/json | array<MenuRole> |
| application/*+json | array<MenuRole> |

Sample JSON request:

```json
{
    "id":  1,
    "menuId":  1,
    "userRoleId":  1,
    "menuCode":  "sample",
    "view":  true,
    "add":  true,
    "edit":  true,
    "delete":  true,
    "approve":  true,
    "menu":  {
                 "id":  1,
                 "code":  "sample",
                 "name":  "sample",
                 "nameEN":  "sample",
                 "nameKO":  "sample",
                 "codeParent":  "sample",
                 "isParent":  true,
                 "order":  1,
                 "note":  "sample"
             },
    "userRole":  {
                     "id":  1,
                     "code":  "sample",
                     "title":  "sample",
                     "note":  "sample",
                     "order":  1,
                     "userCreated":  1,
                     "isNotAllowDelete":  true,
                     "companyCode":  "sample",
                     "parentId":  1
                 }
}
```

#### Responses

| Status | Description | Content-Type | Schema |
|---|---|---|---|
| 200 | Success |  |  |

## Tag: Menus

### GET /api/Menus

- OperationId: 
- Auth (Swagger): No
- Auth (Observed): Yes

#### Parameters

| Name | In | Required | Type | Description |
|---|---|---|---|---|
| CodeParent | query | No | string |  |
| isParent | query | No | boolean |  |
| Page | query | No | integer(int32) |  |
| PageSize | query | No | integer(int32) |  |
| SearchText | query | No | string |  |
| userRoleId | query | No | integer(int32) |  |

#### Responses

| Status | Description | Content-Type | Schema |
|---|---|---|---|
| 200 | Success |  |  |

### POST /api/Menus

- OperationId: 
- Auth (Swagger): No
- Auth (Observed): Yes

#### Request Body

- Required: No

| Content-Type | Schema |
|---|---|
| application/json | MenuViewModel |
| text/json | MenuViewModel |
| application/*+json | MenuViewModel |

Sample JSON request:

```json
{
    "id":  1,
    "code":  "sample",
    "name":  "sample",
    "nameEN":  "sample",
    "nameKO":  "sample",
    "codeParent":  "sample",
    "isParent":  true,
    "order":  1,
    "note":  "sample"
}
```

#### Responses

| Status | Description | Content-Type | Schema |
|---|---|---|---|
| 200 | Success |  |  |

### GET /api/Menus/{id}

- OperationId: 
- Auth (Swagger): No
- Auth (Observed): Yes

#### Parameters

| Name | In | Required | Type | Description |
|---|---|---|---|---|
| id | path | Yes | integer(int32) |  |

#### Responses

| Status | Description | Content-Type | Schema |
|---|---|---|---|
| 200 | Success |  |  |

### PUT /api/Menus/{id}

- OperationId: 
- Auth (Swagger): No
- Auth (Observed): Yes

#### Parameters

| Name | In | Required | Type | Description |
|---|---|---|---|---|
| id | path | Yes | integer(int32) |  |

#### Request Body

- Required: No

| Content-Type | Schema |
|---|---|
| application/json | MenuViewModel |
| text/json | MenuViewModel |
| application/*+json | MenuViewModel |

Sample JSON request:

```json
{
    "id":  1,
    "code":  "sample",
    "name":  "sample",
    "nameEN":  "sample",
    "nameKO":  "sample",
    "codeParent":  "sample",
    "isParent":  true,
    "order":  1,
    "note":  "sample"
}
```

#### Responses

| Status | Description | Content-Type | Schema |
|---|---|---|---|
| 200 | Success |  |  |

### DELETE /api/Menus/{id}

- OperationId: 
- Auth (Swagger): No
- Auth (Observed): Yes

#### Parameters

| Name | In | Required | Type | Description |
|---|---|---|---|---|
| id | path | Yes | integer(int32) |  |

#### Responses

| Status | Description | Content-Type | Schema |
|---|---|---|---|
| 200 | Success |  |  |

### GET /api/Menus/check-role

- OperationId: 
- Auth (Swagger): No
- Auth (Observed): Yes

#### Parameters

| Name | In | Required | Type | Description |
|---|---|---|---|---|
| menuCode | query | No | string |  |

#### Responses

| Status | Description | Content-Type | Schema |
|---|---|---|---|
| 200 | Success |  |  |

### GET /api/Menus/list

- OperationId: 
- Auth (Swagger): No
- Auth (Observed): Yes

#### Parameters

| Name | In | Required | Type | Description |
|---|---|---|---|---|
| CodeParent | query | No | string |  |
| isParent | query | No | boolean |  |
| Page | query | No | integer(int32) |  |
| PageSize | query | No | integer(int32) |  |
| SearchText | query | No | string |  |
| userRoleId | query | No | integer(int32) |  |

#### Responses

| Status | Description | Content-Type | Schema |
|---|---|---|---|
| 200 | Success |  |  |

### GET /api/Menus/user/{userId}

- OperationId: 
- Auth (Swagger): No
- Auth (Observed): Yes

#### Parameters

| Name | In | Required | Type | Description |
|---|---|---|---|---|
| userId | path | Yes | integer(int32) |  |

#### Responses

| Status | Description | Content-Type | Schema |
|---|---|---|---|
| 200 | Success |  |  |

### GET /api/Menus/user/{userId}/permissions

- OperationId: 
- Auth (Swagger): No
- Auth (Observed): Yes

#### Parameters

| Name | In | Required | Type | Description |
|---|---|---|---|---|
| userId | path | Yes | integer(int32) |  |

#### Responses

| Status | Description | Content-Type | Schema |
|---|---|---|---|
| 200 | Success |  |  |

## Tag: PetrolConsumptions

### GET /api/PetrolConsumptions

- OperationId: 
- Auth (Swagger): No
- Auth (Observed): Yes

#### Parameters

| Name | In | Required | Type | Description |
|---|---|---|---|---|
| Page | query | No | integer(int32) |  |
| PageSize | query | No | integer(int32) |  |
| SearchText | query | No | string |  |

#### Responses

| Status | Description | Content-Type | Schema |
|---|---|---|---|
| 200 | Success |  |  |

### POST /api/PetrolConsumptions

- OperationId: 
- Auth (Swagger): No
- Auth (Observed): Yes

#### Request Body

- Required: No

| Content-Type | Schema |
|---|---|
| application/json | PetrolConsumptionModel |
| text/json | PetrolConsumptionModel |
| application/*+json | PetrolConsumptionModel |

Sample JSON request:

```json
{
    "id":  1,
    "date":  "2026-03-13T11:57:27.1615186+07:00",
    "userId":  1,
    "carId":  1,
    "petroPrice":  1,
    "kmFrom":  1,
    "kmTo":  1,
    "locationFrom":  "sample",
    "locationTo":  "sample",
    "advanceAmount":  1,
    "note":  "sample",
    "roadRouteId":  1,
    "points":  {
                   "amount":  1,
                   "policeCheckPointName":  "sample",
                   "isArise":  true
               }
}
```

#### Responses

| Status | Description | Content-Type | Schema |
|---|---|---|---|
| 200 | Success |  |  |

### GET /api/PetrolConsumptions/{id}

- OperationId: 
- Auth (Swagger): No
- Auth (Observed): Yes

#### Parameters

| Name | In | Required | Type | Description |
|---|---|---|---|---|
| id | path | Yes | integer(int32) |  |

#### Responses

| Status | Description | Content-Type | Schema |
|---|---|---|---|
| 200 | Success |  |  |

### PUT /api/PetrolConsumptions/{id}

- OperationId: 
- Auth (Swagger): No
- Auth (Observed): Yes

#### Parameters

| Name | In | Required | Type | Description |
|---|---|---|---|---|
| id | path | Yes | integer(int32) |  |

#### Request Body

- Required: No

| Content-Type | Schema |
|---|---|
| application/json | PetrolConsumptionModel |
| text/json | PetrolConsumptionModel |
| application/*+json | PetrolConsumptionModel |

Sample JSON request:

```json
{
    "id":  1,
    "date":  "2026-03-13T11:57:27.1838286+07:00",
    "userId":  1,
    "carId":  1,
    "petroPrice":  1,
    "kmFrom":  1,
    "kmTo":  1,
    "locationFrom":  "sample",
    "locationTo":  "sample",
    "advanceAmount":  1,
    "note":  "sample",
    "roadRouteId":  1,
    "points":  {
                   "amount":  1,
                   "policeCheckPointName":  "sample",
                   "isArise":  true
               }
}
```

#### Responses

| Status | Description | Content-Type | Schema |
|---|---|---|---|
| 200 | Success |  |  |

### DELETE /api/PetrolConsumptions/{id}

- OperationId: 
- Auth (Swagger): No
- Auth (Observed): Yes

#### Parameters

| Name | In | Required | Type | Description |
|---|---|---|---|---|
| id | path | Yes | integer(int32) |  |

#### Responses

| Status | Description | Content-Type | Schema |
|---|---|---|---|
| 200 | Success |  |  |

### GET /api/PetrolConsumptions/report

- OperationId: 
- Auth (Swagger): No
- Auth (Observed): Yes

#### Parameters

| Name | In | Required | Type | Description |
|---|---|---|---|---|
| CarId | query | No | integer(int32) |  |
| FromDate | query | No | string(date-time) |  |
| ToDate | query | No | string(date-time) |  |
| UserId | query | No | integer(int32) |  |

#### Responses

| Status | Description | Content-Type | Schema |
|---|---|---|---|
| 200 | Success |  |  |

## Tag: PoliceCheckPoints

### GET /api/PoliceCheckPoints

- OperationId: 
- Auth (Swagger): No
- Auth (Observed): Yes

#### Parameters

| Name | In | Required | Type | Description |
|---|---|---|---|---|
| Page | query | No | integer(int32) |  |
| PageSize | query | No | integer(int32) |  |
| SearchText | query | No | string |  |

#### Responses

| Status | Description | Content-Type | Schema |
|---|---|---|---|
| 200 | Success |  |  |

### POST /api/PoliceCheckPoints

- OperationId: 
- Auth (Swagger): No
- Auth (Observed): Yes

#### Request Body

- Required: No

| Content-Type | Schema |
|---|---|
| application/json | PoliceCheckPointModel |
| text/json | PoliceCheckPointModel |
| application/*+json | PoliceCheckPointModel |

Sample JSON request:

```json
{
    "id":  1,
    "code":  "sample",
    "name":  "sample",
    "amount":  1
}
```

#### Responses

| Status | Description | Content-Type | Schema |
|---|---|---|---|
| 200 | Success |  |  |

### GET /api/PoliceCheckPoints/{id}

- OperationId: 
- Auth (Swagger): No
- Auth (Observed): Yes

#### Parameters

| Name | In | Required | Type | Description |
|---|---|---|---|---|
| id | path | Yes | integer(int32) |  |

#### Responses

| Status | Description | Content-Type | Schema |
|---|---|---|---|
| 200 | Success |  |  |

### PUT /api/PoliceCheckPoints/{id}

- OperationId: 
- Auth (Swagger): No
- Auth (Observed): Yes

#### Parameters

| Name | In | Required | Type | Description |
|---|---|---|---|---|
| id | path | Yes | integer(int32) |  |

#### Request Body

- Required: No

| Content-Type | Schema |
|---|---|
| application/json | PoliceCheckPointModel |
| text/json | PoliceCheckPointModel |
| application/*+json | PoliceCheckPointModel |

Sample JSON request:

```json
{
    "id":  1,
    "code":  "sample",
    "name":  "sample",
    "amount":  1
}
```

#### Responses

| Status | Description | Content-Type | Schema |
|---|---|---|---|
| 200 | Success |  |  |

### DELETE /api/PoliceCheckPoints/{id}

- OperationId: 
- Auth (Swagger): No
- Auth (Observed): Yes

#### Parameters

| Name | In | Required | Type | Description |
|---|---|---|---|---|
| id | path | Yes | integer(int32) |  |

#### Responses

| Status | Description | Content-Type | Schema |
|---|---|---|---|
| 200 | Success |  |  |

### GET /api/PoliceCheckPoints/list

- OperationId: 
- Auth (Swagger): No
- Auth (Observed): Yes

#### Responses

| Status | Description | Content-Type | Schema |
|---|---|---|---|
| 200 | Success |  |  |

## Tag: PositionDetails

### GET /api/PositionDetails

- OperationId: 
- Auth (Swagger): No
- Auth (Observed): Yes

#### Responses

| Status | Description | Content-Type | Schema |
|---|---|---|---|
| 200 | Success |  |  |

### POST /api/PositionDetails

- OperationId: 
- Auth (Swagger): No
- Auth (Observed): Yes

#### Request Body

- Required: No

| Content-Type | Schema |
|---|---|
| application/json | PositionDetailRequest |
| text/json | PositionDetailRequest |
| application/*+json | PositionDetailRequest |

Sample JSON request:

```json
{
    "name":  "sample",
    "code":  "sample",
    "departmentId":  1,
    "note":  "sample",
    "order":  1
}
```

#### Responses

| Status | Description | Content-Type | Schema |
|---|---|---|---|
| 200 | Success |  |  |

### GET /api/PositionDetails/{id}

- OperationId: 
- Auth (Swagger): No
- Auth (Observed): Yes

#### Parameters

| Name | In | Required | Type | Description |
|---|---|---|---|---|
| id | path | Yes | integer(int32) |  |

#### Responses

| Status | Description | Content-Type | Schema |
|---|---|---|---|
| 200 | Success |  |  |

### PUT /api/PositionDetails/{id}

- OperationId: 
- Auth (Swagger): No
- Auth (Observed): Yes

#### Parameters

| Name | In | Required | Type | Description |
|---|---|---|---|---|
| id | path | Yes | integer(int32) |  |

#### Request Body

- Required: No

| Content-Type | Schema |
|---|---|
| application/json | PositionDetailRequest |
| text/json | PositionDetailRequest |
| application/*+json | PositionDetailRequest |

Sample JSON request:

```json
{
    "name":  "sample",
    "code":  "sample",
    "departmentId":  1,
    "note":  "sample",
    "order":  1
}
```

#### Responses

| Status | Description | Content-Type | Schema |
|---|---|---|---|
| 200 | Success |  |  |

### DELETE /api/PositionDetails/{id}

- OperationId: 
- Auth (Swagger): No
- Auth (Observed): Yes

#### Parameters

| Name | In | Required | Type | Description |
|---|---|---|---|---|
| id | path | Yes | integer(int32) |  |

#### Responses

| Status | Description | Content-Type | Schema |
|---|---|---|---|
| 200 | Success |  |  |

## Tag: Relatives

### GET /api/Relatives

- OperationId: 
- Auth (Swagger): No
- Auth (Observed): Yes

#### Responses

| Status | Description | Content-Type | Schema |
|---|---|---|---|
| 200 | Success |  |  |

### POST /api/Relatives

- OperationId: 
- Auth (Swagger): No
- Auth (Observed): Yes

#### Request Body

- Required: No

| Content-Type | Schema |
|---|---|
| application/json | RelativeRequest |
| text/json | RelativeRequest |
| application/*+json | RelativeRequest |

Sample JSON request:

```json
{
    "userId":  1,
    "name":  "sample",
    "relationship":  "sample",
    "phone":  "sample",
    "address":  "sample",
    "note":  "sample"
}
```

#### Responses

| Status | Description | Content-Type | Schema |
|---|---|---|---|
| 200 | Success |  |  |

### GET /api/Relatives/{id}

- OperationId: 
- Auth (Swagger): No
- Auth (Observed): Yes

#### Parameters

| Name | In | Required | Type | Description |
|---|---|---|---|---|
| id | path | Yes | integer(int32) |  |

#### Responses

| Status | Description | Content-Type | Schema |
|---|---|---|---|
| 200 | Success |  |  |

### PUT /api/Relatives/{id}

- OperationId: 
- Auth (Swagger): No
- Auth (Observed): Yes

#### Parameters

| Name | In | Required | Type | Description |
|---|---|---|---|---|
| id | path | Yes | integer(int32) |  |

#### Request Body

- Required: No

| Content-Type | Schema |
|---|---|
| application/json | RelativeRequest |
| text/json | RelativeRequest |
| application/*+json | RelativeRequest |

Sample JSON request:

```json
{
    "userId":  1,
    "name":  "sample",
    "relationship":  "sample",
    "phone":  "sample",
    "address":  "sample",
    "note":  "sample"
}
```

#### Responses

| Status | Description | Content-Type | Schema |
|---|---|---|---|
| 200 | Success |  |  |

### DELETE /api/Relatives/{id}

- OperationId: 
- Auth (Swagger): No
- Auth (Observed): Yes

#### Parameters

| Name | In | Required | Type | Description |
|---|---|---|---|---|
| id | path | Yes | integer(int32) |  |

#### Responses

| Status | Description | Content-Type | Schema |
|---|---|---|---|
| 200 | Success |  |  |

### GET /api/Relatives/user/{userId}

- OperationId: 
- Auth (Swagger): No
- Auth (Observed): Yes

#### Parameters

| Name | In | Required | Type | Description |
|---|---|---|---|---|
| userId | path | Yes | integer(int32) |  |

#### Responses

| Status | Description | Content-Type | Schema |
|---|---|---|---|
| 200 | Success |  |  |

## Tag: RoadRoutes

### GET /api/RoadRoutes

- OperationId: 
- Auth (Swagger): No
- Auth (Observed): Yes

#### Parameters

| Name | In | Required | Type | Description |
|---|---|---|---|---|
| Page | query | No | integer(int32) |  |
| PageSize | query | No | integer(int32) |  |
| SearchText | query | No | string |  |

#### Responses

| Status | Description | Content-Type | Schema |
|---|---|---|---|
| 200 | Success |  |  |

### POST /api/RoadRoutes

- OperationId: 
- Auth (Swagger): No
- Auth (Observed): Yes

#### Request Body

- Required: No

| Content-Type | Schema |
|---|---|
| application/json | RoadRouteModel |
| text/json | RoadRouteModel |
| application/*+json | RoadRouteModel |

Sample JSON request:

```json
{
    "id":  1,
    "code":  "sample",
    "name":  "sample",
    "roadRouteDetail":  "sample",
    "policeCheckPointIdStr":  "sample",
    "numberOfTrips":  1
}
```

#### Responses

| Status | Description | Content-Type | Schema |
|---|---|---|---|
| 200 | Success |  |  |

### GET /api/RoadRoutes/{id}

- OperationId: 
- Auth (Swagger): No
- Auth (Observed): Yes

#### Parameters

| Name | In | Required | Type | Description |
|---|---|---|---|---|
| id | path | Yes | integer(int32) |  |

#### Responses

| Status | Description | Content-Type | Schema |
|---|---|---|---|
| 200 | Success |  |  |

### PUT /api/RoadRoutes/{id}

- OperationId: 
- Auth (Swagger): No
- Auth (Observed): Yes

#### Parameters

| Name | In | Required | Type | Description |
|---|---|---|---|---|
| id | path | Yes | integer(int32) |  |

#### Request Body

- Required: No

| Content-Type | Schema |
|---|---|
| application/json | RoadRouteModel |
| text/json | RoadRouteModel |
| application/*+json | RoadRouteModel |

Sample JSON request:

```json
{
    "id":  1,
    "code":  "sample",
    "name":  "sample",
    "roadRouteDetail":  "sample",
    "policeCheckPointIdStr":  "sample",
    "numberOfTrips":  1
}
```

#### Responses

| Status | Description | Content-Type | Schema |
|---|---|---|---|
| 200 | Success |  |  |

### DELETE /api/RoadRoutes/{id}

- OperationId: 
- Auth (Swagger): No
- Auth (Observed): Yes

#### Parameters

| Name | In | Required | Type | Description |
|---|---|---|---|---|
| id | path | Yes | integer(int32) |  |

#### Responses

| Status | Description | Content-Type | Schema |
|---|---|---|---|
| 200 | Success |  |  |

## Tag: SalaryTypes

### GET /api/SalaryTypes

- OperationId: 
- Auth (Swagger): No
- Auth (Observed): Yes

#### Responses

| Status | Description | Content-Type | Schema |
|---|---|---|---|
| 200 | Success |  |  |

### POST /api/SalaryTypes

- OperationId: 
- Auth (Swagger): No
- Auth (Observed): Yes

#### Request Body

- Required: No

| Content-Type | Schema |
|---|---|
| application/json | SalaryTypeRequest |
| text/json | SalaryTypeRequest |
| application/*+json | SalaryTypeRequest |

Sample JSON request:

```json
{
    "name":  "sample",
    "code":  "sample",
    "baseAmount":  1,
    "formula":  "sample",
    "note":  "sample"
}
```

#### Responses

| Status | Description | Content-Type | Schema |
|---|---|---|---|
| 200 | Success |  |  |

### GET /api/SalaryTypes/{id}

- OperationId: 
- Auth (Swagger): No
- Auth (Observed): Yes

#### Parameters

| Name | In | Required | Type | Description |
|---|---|---|---|---|
| id | path | Yes | integer(int32) |  |

#### Responses

| Status | Description | Content-Type | Schema |
|---|---|---|---|
| 200 | Success |  |  |

### PUT /api/SalaryTypes/{id}

- OperationId: 
- Auth (Swagger): No
- Auth (Observed): Yes

#### Parameters

| Name | In | Required | Type | Description |
|---|---|---|---|---|
| id | path | Yes | integer(int32) |  |

#### Request Body

- Required: No

| Content-Type | Schema |
|---|---|
| application/json | SalaryTypeRequest |
| text/json | SalaryTypeRequest |
| application/*+json | SalaryTypeRequest |

Sample JSON request:

```json
{
    "name":  "sample",
    "code":  "sample",
    "baseAmount":  1,
    "formula":  "sample",
    "note":  "sample"
}
```

#### Responses

| Status | Description | Content-Type | Schema |
|---|---|---|---|
| 200 | Success |  |  |

### DELETE /api/SalaryTypes/{id}

- OperationId: 
- Auth (Swagger): No
- Auth (Observed): Yes

#### Parameters

| Name | In | Required | Type | Description |
|---|---|---|---|---|
| id | path | Yes | integer(int32) |  |

#### Responses

| Status | Description | Content-Type | Schema |
|---|---|---|---|
| 200 | Success |  |  |

## Tag: TimeKeeping

### GET /api/TimeKeeping

- OperationId: 
- Auth (Swagger): No
- Auth (Observed): Yes

#### Parameters

| Name | In | Required | Type | Description |
|---|---|---|---|---|
| from | query | No | string(date-time) |  |
| to | query | No | string(date-time) |  |
| userId | query | No | integer(int32) |  |

#### Responses

| Status | Description | Content-Type | Schema |
|---|---|---|---|
| 200 | Success |  |  |

### POST /api/TimeKeeping

- OperationId: 
- Auth (Swagger): No
- Auth (Observed): Yes

#### Request Body

- Required: No

| Content-Type | Schema |
|---|---|
| application/json | TimeKeepingEntryRequest |
| text/json | TimeKeepingEntryRequest |
| application/*+json | TimeKeepingEntryRequest |

Sample JSON request:

```json
{
    "userId":  1,
    "workDate":  "2026-03-13T11:57:27.2590920+07:00",
    "checkIn":  "2026-03-13T11:57:27.2600963+07:00",
    "checkOut":  "2026-03-13T11:57:27.2600963+07:00",
    "workingHours":  1,
    "note":  "sample"
}
```

#### Responses

| Status | Description | Content-Type | Schema |
|---|---|---|---|
| 200 | Success |  |  |

### GET /api/TimeKeeping/{id}

- OperationId: 
- Auth (Swagger): No
- Auth (Observed): Yes

#### Parameters

| Name | In | Required | Type | Description |
|---|---|---|---|---|
| id | path | Yes | integer(int32) |  |

#### Responses

| Status | Description | Content-Type | Schema |
|---|---|---|---|
| 200 | Success |  |  |

### PUT /api/TimeKeeping/{id}

- OperationId: 
- Auth (Swagger): No
- Auth (Observed): Yes

#### Parameters

| Name | In | Required | Type | Description |
|---|---|---|---|---|
| id | path | Yes | integer(int32) |  |

#### Request Body

- Required: No

| Content-Type | Schema |
|---|---|
| application/json | TimeKeepingEntryRequest |
| text/json | TimeKeepingEntryRequest |
| application/*+json | TimeKeepingEntryRequest |

Sample JSON request:

```json
{
    "userId":  1,
    "workDate":  "2026-03-13T11:57:27.2640895+07:00",
    "checkIn":  "2026-03-13T11:57:27.2640895+07:00",
    "checkOut":  "2026-03-13T11:57:27.2640895+07:00",
    "workingHours":  1,
    "note":  "sample"
}
```

#### Responses

| Status | Description | Content-Type | Schema |
|---|---|---|---|
| 200 | Success |  |  |

### DELETE /api/TimeKeeping/{id}

- OperationId: 
- Auth (Swagger): No
- Auth (Observed): Yes

#### Parameters

| Name | In | Required | Type | Description |
|---|---|---|---|---|
| id | path | Yes | integer(int32) |  |

#### Responses

| Status | Description | Content-Type | Schema |
|---|---|---|---|
| 200 | Success |  |  |

## Tag: UserContractHistories

### GET /api/UserContractHistories

- OperationId: 
- Auth (Swagger): No
- Auth (Observed): Yes

#### Responses

| Status | Description | Content-Type | Schema |
|---|---|---|---|
| 200 | Success |  |  |

### POST /api/UserContractHistories

- OperationId: 
- Auth (Swagger): No
- Auth (Observed): Yes

#### Request Body

- Required: No

| Content-Type | Schema |
|---|---|
| application/json | UserContractHistoryRequest |
| text/json | UserContractHistoryRequest |
| application/*+json | UserContractHistoryRequest |

Sample JSON request:

```json
{
    "userId":  1,
    "contractTypeId":  1,
    "signedDate":  "2026-03-13T11:57:27.2661497+07:00",
    "startDate":  "2026-03-13T11:57:27.2661497+07:00",
    "endDate":  "2026-03-13T11:57:27.2661497+07:00",
    "fileUrl":  "sample",
    "note":  "sample"
}
```

#### Responses

| Status | Description | Content-Type | Schema |
|---|---|---|---|
| 200 | Success |  |  |

### GET /api/UserContractHistories/{id}

- OperationId: 
- Auth (Swagger): No
- Auth (Observed): Yes

#### Parameters

| Name | In | Required | Type | Description |
|---|---|---|---|---|
| id | path | Yes | integer(int32) |  |

#### Responses

| Status | Description | Content-Type | Schema |
|---|---|---|---|
| 200 | Success |  |  |

### PUT /api/UserContractHistories/{id}

- OperationId: 
- Auth (Swagger): No
- Auth (Observed): Yes

#### Parameters

| Name | In | Required | Type | Description |
|---|---|---|---|---|
| id | path | Yes | integer(int32) |  |

#### Request Body

- Required: No

| Content-Type | Schema |
|---|---|
| application/json | UserContractHistoryRequest |
| text/json | UserContractHistoryRequest |
| application/*+json | UserContractHistoryRequest |

Sample JSON request:

```json
{
    "userId":  1,
    "contractTypeId":  1,
    "signedDate":  "2026-03-13T11:57:27.2763248+07:00",
    "startDate":  "2026-03-13T11:57:27.2763248+07:00",
    "endDate":  "2026-03-13T11:57:27.2763248+07:00",
    "fileUrl":  "sample",
    "note":  "sample"
}
```

#### Responses

| Status | Description | Content-Type | Schema |
|---|---|---|---|
| 200 | Success |  |  |

### DELETE /api/UserContractHistories/{id}

- OperationId: 
- Auth (Swagger): No
- Auth (Observed): Yes

#### Parameters

| Name | In | Required | Type | Description |
|---|---|---|---|---|
| id | path | Yes | integer(int32) |  |

#### Responses

| Status | Description | Content-Type | Schema |
|---|---|---|---|
| 200 | Success |  |  |

### GET /api/UserContractHistories/user/{userId}

- OperationId: 
- Auth (Swagger): No
- Auth (Observed): Yes

#### Parameters

| Name | In | Required | Type | Description |
|---|---|---|---|---|
| userId | path | Yes | integer(int32) |  |

#### Responses

| Status | Description | Content-Type | Schema |
|---|---|---|---|
| 200 | Success |  |  |

## Tag: UserMenus

### GET /api/UserMenus/{userId}

- OperationId: 
- Auth (Swagger): No
- Auth (Observed): Yes

#### Parameters

| Name | In | Required | Type | Description |
|---|---|---|---|---|
| userId | path | Yes | integer(int32) |  |

#### Responses

| Status | Description | Content-Type | Schema |
|---|---|---|---|
| 200 | Success |  |  |

### POST /api/UserMenus/{userId}/assign

- OperationId: 
- Auth (Swagger): No
- Auth (Observed): Yes

#### Parameters

| Name | In | Required | Type | Description |
|---|---|---|---|---|
| userId | path | Yes | integer(int32) |  |

#### Request Body

- Required: No

| Content-Type | Schema |
|---|---|
| application/json | array<UserMenuAssignDto> |
| text/json | array<UserMenuAssignDto> |
| application/*+json | array<UserMenuAssignDto> |

Sample JSON request:

```json
{
    "menuId":  1,
    "menuCode":  "sample",
    "view":  true,
    "add":  true,
    "edit":  true,
    "delete":  true
}
```

#### Responses

| Status | Description | Content-Type | Schema |
|---|---|---|---|
| 200 | Success |  |  |

### DELETE /api/UserMenus/{userId}/clear

- OperationId: 
- Auth (Swagger): No
- Auth (Observed): Yes

#### Parameters

| Name | In | Required | Type | Description |
|---|---|---|---|---|
| userId | path | Yes | integer(int32) |  |

#### Responses

| Status | Description | Content-Type | Schema |
|---|---|---|---|
| 200 | Success |  |  |

### DELETE /api/UserMenus/{userId}/menu/{menuId}

- OperationId: 
- Auth (Swagger): No
- Auth (Observed): Yes

#### Parameters

| Name | In | Required | Type | Description |
|---|---|---|---|---|
| menuId | path | Yes | integer(int32) |  |
| userId | path | Yes | integer(int32) |  |

#### Responses

| Status | Description | Content-Type | Schema |
|---|---|---|---|
| 200 | Success |  |  |

### GET /api/UserMenus/{userId}/permissions

- OperationId: 
- Auth (Swagger): No
- Auth (Observed): Yes

#### Parameters

| Name | In | Required | Type | Description |
|---|---|---|---|---|
| userId | path | Yes | integer(int32) |  |

#### Responses

| Status | Description | Content-Type | Schema |
|---|---|---|---|
| 200 | Success |  |  |

## Tag: UserRoles

### GET /api/UserRoles

- OperationId: 
- Auth (Swagger): No
- Auth (Observed): Yes

#### Parameters

| Name | In | Required | Type | Description |
|---|---|---|---|---|
| Page | query | No | integer(int32) |  |
| PageSize | query | No | integer(int32) |  |
| SearchText | query | No | string |  |

#### Responses

| Status | Description | Content-Type | Schema |
|---|---|---|---|
| 200 | Success |  |  |

### POST /api/UserRoles

- OperationId: 
- Auth (Swagger): No
- Auth (Observed): Yes

#### Request Body

- Required: No

| Content-Type | Schema |
|---|---|
| application/json | UserRole |
| text/json | UserRole |
| application/*+json | UserRole |

Sample JSON request:

```json
{
    "id":  1,
    "code":  "sample",
    "title":  "sample",
    "note":  "sample",
    "order":  1,
    "userCreated":  1,
    "isNotAllowDelete":  true,
    "companyCode":  "sample",
    "parentId":  1
}
```

#### Responses

| Status | Description | Content-Type | Schema |
|---|---|---|---|
| 200 | Success |  |  |

### GET /api/UserRoles/{id}

- OperationId: 
- Auth (Swagger): No
- Auth (Observed): Yes

#### Parameters

| Name | In | Required | Type | Description |
|---|---|---|---|---|
| id | path | Yes | integer(int32) |  |

#### Responses

| Status | Description | Content-Type | Schema |
|---|---|---|---|
| 200 | Success |  |  |

### PUT /api/UserRoles/{id}

- OperationId: 
- Auth (Swagger): No
- Auth (Observed): Yes

#### Parameters

| Name | In | Required | Type | Description |
|---|---|---|---|---|
| id | path | Yes | string |  |

#### Request Body

- Required: No

| Content-Type | Schema |
|---|---|
| application/json | UserRole |
| text/json | UserRole |
| application/*+json | UserRole |

Sample JSON request:

```json
{
    "id":  1,
    "code":  "sample",
    "title":  "sample",
    "note":  "sample",
    "order":  1,
    "userCreated":  1,
    "isNotAllowDelete":  true,
    "companyCode":  "sample",
    "parentId":  1
}
```

#### Responses

| Status | Description | Content-Type | Schema |
|---|---|---|---|
| 200 | Success |  |  |

### DELETE /api/UserRoles/{id}

- OperationId: 
- Auth (Swagger): No
- Auth (Observed): Yes

#### Parameters

| Name | In | Required | Type | Description |
|---|---|---|---|---|
| id | path | Yes | integer(int32) |  |

#### Responses

| Status | Description | Content-Type | Schema |
|---|---|---|---|
| 200 | Success |  |  |

### GET /api/UserRoles/list

- OperationId: 
- Auth (Swagger): No
- Auth (Observed): Yes

#### Parameters

| Name | In | Required | Type | Description |
|---|---|---|---|---|
| companyCode | query | No | string |  |

#### Responses

| Status | Description | Content-Type | Schema |
|---|---|---|---|
| 200 | Success |  |  |

## Tag: Users

### GET /api/Users

- OperationId: 
- Auth (Swagger): No
- Auth (Observed): Yes

#### Parameters

| Name | In | Required | Type | Description |
|---|---|---|---|---|
| Birthday | query | No | string(date-time) |  |
| Certificateid | query | No | integer(int32) |  |
| CompanyCode | query | No | string |  |
| Degreeid | query | No | integer(int32) |  |
| DepartmentId | query | No | integer(int32) |  |
| EndDate | query | No | string(date-time) |  |
| Gender | query | No | integer(int32) |  |
| Ids | query | No | array<integer(int32)> |  |
| Month | query | No | integer(int32) |  |
| Page | query | No | integer(int32) |  |
| PageSize | query | No | integer(int32) |  |
| PositionId | query | No | integer(int32) |  |
| Quit | query | No | boolean |  |
| RequestPassword | query | No | boolean |  |
| SearchText | query | No | string |  |
| StartDate | query | No | string(date-time) |  |
| Targetid | query | No | integer(int32) |  |
| Warehouseid | query | No | integer(int32) |  |

#### Responses

| Status | Description | Content-Type | Schema |
|---|---|---|---|
| 200 | Success |  |  |

### POST /api/Users

- OperationId: 
- Auth (Swagger): No
- Auth (Observed): Yes

#### Request Body

- Required: No

| Content-Type | Schema |
|---|---|
| application/json | CreateUserRequest |
| text/json | CreateUserRequest |
| application/*+json | CreateUserRequest |

Sample JSON request:

```json
{
    "username":  "sample",
    "password":  "sample",
    "fullName":  "sample",
    "email":  "sample",
    "phone":  "sample",
    "userRoleIds":  "sample",
    "departmentId":  1,
    "positionDetailId":  1,
    "gender":  1,
    "birthDay":  "2026-03-13T11:57:27.3168229+07:00",
    "address":  "sample",
    "companyCode":  "sample"
}
```

#### Responses

| Status | Description | Content-Type | Schema |
|---|---|---|---|
| 200 | Success |  |  |

### GET /api/Users/{id}

- OperationId: 
- Auth (Swagger): No
- Auth (Observed): Yes

#### Parameters

| Name | In | Required | Type | Description |
|---|---|---|---|---|
| id | path | Yes | integer(int32) |  |

#### Responses

| Status | Description | Content-Type | Schema |
|---|---|---|---|
| 200 | Success |  |  |

### PUT /api/Users/{id}

- OperationId: 
- Auth (Swagger): No
- Auth (Observed): Yes

#### Parameters

| Name | In | Required | Type | Description |
|---|---|---|---|---|
| id | path | Yes | integer(int32) |  |

#### Request Body

- Required: No

| Content-Type | Schema |
|---|---|
| application/json | CreateUserRequest |
| text/json | CreateUserRequest |
| application/*+json | CreateUserRequest |

Sample JSON request:

```json
{
    "username":  "sample",
    "password":  "sample",
    "fullName":  "sample",
    "email":  "sample",
    "phone":  "sample",
    "userRoleIds":  "sample",
    "departmentId":  1,
    "positionDetailId":  1,
    "gender":  1,
    "birthDay":  "2026-03-13T11:57:27.3228332+07:00",
    "address":  "sample",
    "companyCode":  "sample"
}
```

#### Responses

| Status | Description | Content-Type | Schema |
|---|---|---|---|
| 200 | Success |  |  |

### DELETE /api/Users/{id}

- OperationId: 
- Auth (Swagger): No
- Auth (Observed): Yes

#### Parameters

| Name | In | Required | Type | Description |
|---|---|---|---|---|
| id | path | Yes | integer(int32) |  |

#### Responses

| Status | Description | Content-Type | Schema |
|---|---|---|---|
| 200 | Success |  |  |

### GET /api/Users/getAllUserActive

- OperationId: 
- Auth (Swagger): No
- Auth (Observed): Yes

#### Responses

| Status | Description | Content-Type | Schema |
|---|---|---|---|
| 200 | Success |  |  |

### POST /api/Users/getallusername

- OperationId: 
- Auth (Swagger): No
- Auth (Observed): Yes

#### Responses

| Status | Description | Content-Type | Schema |
|---|---|---|---|
| 200 | Success |  |  |

### GET /api/Users/get-total-reset-pass

- OperationId: 
- Auth (Swagger): No
- Auth (Observed): Yes

#### Responses

| Status | Description | Content-Type | Schema |
|---|---|---|---|
| 200 | Success |  |  |

### POST /api/Users/resetPassword

- OperationId: 
- Auth (Swagger): No
- Auth (Observed): Yes

#### Request Body

- Required: No

| Content-Type | Schema |
|---|---|
| application/json | array<integer(int32)> |
| text/json | array<integer(int32)> |
| application/*+json | array<integer(int32)> |

Sample JSON request:

```json
1
```

#### Responses

| Status | Description | Content-Type | Schema |
|---|---|---|---|
| 200 | Success |  |  |

### PUT /api/Users/update-current-year

- OperationId: 
- Auth (Swagger): No
- Auth (Observed): Yes

#### Parameters

| Name | In | Required | Type | Description |
|---|---|---|---|---|
| year | query | No | integer(int32) |  |

#### Responses

| Status | Description | Content-Type | Schema |
|---|---|---|---|
| 200 | Success |  |  |

### GET /api/Users/user-not-roles

- OperationId: 
- Auth (Swagger): No
- Auth (Observed): Yes

#### Responses

| Status | Description | Content-Type | Schema |
|---|---|---|---|
| 200 | Success |  |  |

## Tag: WareHouseFloors

### GET /api/WareHouseFloors

- OperationId: 
- Auth (Swagger): No
- Auth (Observed): Yes

#### Parameters

| Name | In | Required | Type | Description |
|---|---|---|---|---|
| Page | query | No | integer(int32) |  |
| PageSize | query | No | integer(int32) |  |
| SearchText | query | No | string |  |

#### Responses

| Status | Description | Content-Type | Schema |
|---|---|---|---|
| 200 | Success |  |  |

### POST /api/WareHouseFloors

- OperationId: 
- Auth (Swagger): No
- Auth (Observed): Yes

#### Request Body

- Required: No

| Content-Type | Schema |
|---|---|
| application/json | WarehouseFloorSetterModel |
| text/json | WarehouseFloorSetterModel |
| application/*+json | WarehouseFloorSetterModel |

Sample JSON request:

```json
{
    "id":  1,
    "code":  "sample",
    "name":  "sample",
    "note":  "sample",
    "positionIds":  1
}
```

#### Responses

| Status | Description | Content-Type | Schema |
|---|---|---|---|
| 200 | Success |  |  |

### GET /api/WareHouseFloors/{id}

- OperationId: 
- Auth (Swagger): No
- Auth (Observed): Yes

#### Parameters

| Name | In | Required | Type | Description |
|---|---|---|---|---|
| id | path | Yes | integer(int32) |  |

#### Responses

| Status | Description | Content-Type | Schema |
|---|---|---|---|
| 200 | Success |  |  |

### PUT /api/WareHouseFloors/{id}

- OperationId: 
- Auth (Swagger): No
- Auth (Observed): Yes

#### Parameters

| Name | In | Required | Type | Description |
|---|---|---|---|---|
| id | path | Yes | integer(int32) |  |

#### Request Body

- Required: No

| Content-Type | Schema |
|---|---|
| application/json | WarehouseFloorSetterModel |
| text/json | WarehouseFloorSetterModel |
| application/*+json | WarehouseFloorSetterModel |

Sample JSON request:

```json
{
    "id":  1,
    "code":  "sample",
    "name":  "sample",
    "note":  "sample",
    "positionIds":  1
}
```

#### Responses

| Status | Description | Content-Type | Schema |
|---|---|---|---|
| 200 | Success |  |  |

### DELETE /api/WareHouseFloors/{id}

- OperationId: 
- Auth (Swagger): No
- Auth (Observed): Yes

#### Parameters

| Name | In | Required | Type | Description |
|---|---|---|---|---|
| id | path | Yes | integer(int32) |  |

#### Responses

| Status | Description | Content-Type | Schema |
|---|---|---|---|
| 200 | Success |  |  |

### GET /api/WareHouseFloors/list

- OperationId: 
- Auth (Swagger): No
- Auth (Observed): Yes

#### Responses

| Status | Description | Content-Type | Schema |
|---|---|---|---|
| 200 | Success |  |  |

## Tag: WareHousePositions

### GET /api/WareHousePositions

- OperationId: 
- Auth (Swagger): No
- Auth (Observed): Yes

#### Parameters

| Name | In | Required | Type | Description |
|---|---|---|---|---|
| Page | query | No | integer(int32) |  |
| PageSize | query | No | integer(int32) |  |
| SearchText | query | No | string |  |

#### Responses

| Status | Description | Content-Type | Schema |
|---|---|---|---|
| 200 | Success |  |  |

### POST /api/WareHousePositions

- OperationId: 
- Auth (Swagger): No
- Auth (Observed): Yes

#### Request Body

- Required: No

| Content-Type | Schema |
|---|---|
| application/json | WareHousePosition |
| text/json | WareHousePosition |
| application/*+json | WareHousePosition |

Sample JSON request:

```json
{
    "id":  1,
    "code":  "sample",
    "name":  "sample",
    "note":  "sample",
    "createdDate":  "2026-03-13T11:57:27.3481711+07:00",
    "updatedDate":  "2026-03-13T11:57:27.3481711+07:00",
    "userCreated":  1,
    "userUpdated":  1,
    "isDeleted":  true
}
```

#### Responses

| Status | Description | Content-Type | Schema |
|---|---|---|---|
| 200 | Success |  |  |

### GET /api/WareHousePositions/{id}

- OperationId: 
- Auth (Swagger): No
- Auth (Observed): Yes

#### Parameters

| Name | In | Required | Type | Description |
|---|---|---|---|---|
| id | path | Yes | integer(int32) |  |

#### Responses

| Status | Description | Content-Type | Schema |
|---|---|---|---|
| 200 | Success |  |  |

### PUT /api/WareHousePositions/{id}

- OperationId: 
- Auth (Swagger): No
- Auth (Observed): Yes

#### Parameters

| Name | In | Required | Type | Description |
|---|---|---|---|---|
| id | path | Yes | integer(int32) |  |

#### Request Body

- Required: No

| Content-Type | Schema |
|---|---|
| application/json | WareHousePosition |
| text/json | WareHousePosition |
| application/*+json | WareHousePosition |

Sample JSON request:

```json
{
    "id":  1,
    "code":  "sample",
    "name":  "sample",
    "note":  "sample",
    "createdDate":  "2026-03-13T11:57:27.3583497+07:00",
    "updatedDate":  "2026-03-13T11:57:27.3583497+07:00",
    "userCreated":  1,
    "userUpdated":  1,
    "isDeleted":  true
}
```

#### Responses

| Status | Description | Content-Type | Schema |
|---|---|---|---|
| 200 | Success |  |  |

### DELETE /api/WareHousePositions/{id}

- OperationId: 
- Auth (Swagger): No
- Auth (Observed): Yes

#### Parameters

| Name | In | Required | Type | Description |
|---|---|---|---|---|
| id | path | Yes | integer(int32) |  |

#### Responses

| Status | Description | Content-Type | Schema |
|---|---|---|---|
| 200 | Success |  |  |

### GET /api/WareHousePositions/list

- OperationId: 
- Auth (Swagger): No
- Auth (Observed): Yes

#### Responses

| Status | Description | Content-Type | Schema |
|---|---|---|---|
| 200 | Success |  |  |

## Tag: Warehouses

### GET /api/Warehouses

- OperationId: 
- Auth (Swagger): No
- Auth (Observed): Yes

#### Parameters

| Name | In | Required | Type | Description |
|---|---|---|---|---|
| Page | query | No | integer(int32) |  |
| PageSize | query | No | integer(int32) |  |
| SearchText | query | No | string |  |

#### Responses

| Status | Description | Content-Type | Schema |
|---|---|---|---|
| 200 | Success |  |  |

### POST /api/Warehouses

- OperationId: 
- Auth (Swagger): No
- Auth (Observed): Yes

#### Parameters

| Name | In | Required | Type | Description |
|---|---|---|---|---|
| yearFilter | header | No | integer(int32) |  |

#### Request Body

- Required: No

| Content-Type | Schema |
|---|---|
| application/json | WarehouseSetterModel |
| text/json | WarehouseSetterModel |
| application/*+json | WarehouseSetterModel |

Sample JSON request:

```json
{
    "id":  1,
    "branchId":  1,
    "name":  "sample",
    "code":  "sample",
    "managerName":  "sample",
    "isSyncChartOfAccount":  true,
    "shelveIds":  1
}
```

#### Responses

| Status | Description | Content-Type | Schema |
|---|---|---|---|
| 200 | Success |  |  |

### GET /api/Warehouses/{id}

- OperationId: 
- Auth (Swagger): No
- Auth (Observed): Yes

#### Parameters

| Name | In | Required | Type | Description |
|---|---|---|---|---|
| id | path | Yes | integer(int32) |  |

#### Responses

| Status | Description | Content-Type | Schema |
|---|---|---|---|
| 200 | Success |  |  |

### PUT /api/Warehouses/{id}

- OperationId: 
- Auth (Swagger): No
- Auth (Observed): Yes

#### Parameters

| Name | In | Required | Type | Description |
|---|---|---|---|---|
| yearFilter | header | No | integer(int32) |  |
| id | path | Yes | integer(int32) |  |

#### Request Body

- Required: No

| Content-Type | Schema |
|---|---|
| application/json | WarehouseSetterModel |
| text/json | WarehouseSetterModel |
| application/*+json | WarehouseSetterModel |

Sample JSON request:

```json
{
    "id":  1,
    "branchId":  1,
    "name":  "sample",
    "code":  "sample",
    "managerName":  "sample",
    "isSyncChartOfAccount":  true,
    "shelveIds":  1
}
```

#### Responses

| Status | Description | Content-Type | Schema |
|---|---|---|---|
| 200 | Success |  |  |

### DELETE /api/Warehouses/{id}

- OperationId: 
- Auth (Swagger): No
- Auth (Observed): Yes

#### Parameters

| Name | In | Required | Type | Description |
|---|---|---|---|---|
| id | path | Yes | integer(int32) |  |

#### Responses

| Status | Description | Content-Type | Schema |
|---|---|---|---|
| 200 | Success |  |  |

### GET /api/Warehouses/list

- OperationId: 
- Auth (Swagger): No
- Auth (Observed): Yes

#### Responses

| Status | Description | Content-Type | Schema |
|---|---|---|---|
| 200 | Success |  |  |

## Tag: WareHouseShelves

### GET /api/WareHouseShelves

- OperationId: 
- Auth (Swagger): No
- Auth (Observed): Yes

#### Parameters

| Name | In | Required | Type | Description |
|---|---|---|---|---|
| Page | query | No | integer(int32) |  |
| PageSize | query | No | integer(int32) |  |
| SearchText | query | No | string |  |

#### Responses

| Status | Description | Content-Type | Schema |
|---|---|---|---|
| 200 | Success |  |  |

### POST /api/WareHouseShelves

- OperationId: 
- Auth (Swagger): No
- Auth (Observed): Yes

#### Request Body

- Required: No

| Content-Type | Schema |
|---|---|
| application/json | WarehouseShelvesSetterModel |
| text/json | WarehouseShelvesSetterModel |
| application/*+json | WarehouseShelvesSetterModel |

Sample JSON request:

```json
{
    "id":  1,
    "code":  "sample",
    "name":  "sample",
    "note":  "sample",
    "orderHorizontal":  1,
    "orderVertical":  1,
    "floorIds":  1
}
```

#### Responses

| Status | Description | Content-Type | Schema |
|---|---|---|---|
| 200 | Success |  |  |

### GET /api/WareHouseShelves/{id}

- OperationId: 
- Auth (Swagger): No
- Auth (Observed): Yes

#### Parameters

| Name | In | Required | Type | Description |
|---|---|---|---|---|
| id | path | Yes | integer(int32) |  |

#### Responses

| Status | Description | Content-Type | Schema |
|---|---|---|---|
| 200 | Success |  |  |

### PUT /api/WareHouseShelves/{id}

- OperationId: 
- Auth (Swagger): No
- Auth (Observed): Yes

#### Parameters

| Name | In | Required | Type | Description |
|---|---|---|---|---|
| id | path | Yes | integer(int32) |  |

#### Request Body

- Required: No

| Content-Type | Schema |
|---|---|
| application/json | WarehouseShelvesSetterModel |
| text/json | WarehouseShelvesSetterModel |
| application/*+json | WarehouseShelvesSetterModel |

Sample JSON request:

```json
{
    "id":  1,
    "code":  "sample",
    "name":  "sample",
    "note":  "sample",
    "orderHorizontal":  1,
    "orderVertical":  1,
    "floorIds":  1
}
```

#### Responses

| Status | Description | Content-Type | Schema |
|---|---|---|---|
| 200 | Success |  |  |

### DELETE /api/WareHouseShelves/{id}

- OperationId: 
- Auth (Swagger): No
- Auth (Observed): Yes

#### Parameters

| Name | In | Required | Type | Description |
|---|---|---|---|---|
| id | path | Yes | integer(int32) |  |

#### Responses

| Status | Description | Content-Type | Schema |
|---|---|---|---|
| 200 | Success |  |  |

### GET /api/WareHouseShelves/list

- OperationId: 
- Auth (Swagger): No
- Auth (Observed): Yes

#### Responses

| Status | Description | Content-Type | Schema |
|---|---|---|---|
| 200 | Success |  |  |

## Tag: WebAuth

### POST /api/WebAuth/change-pass-word/{id}

- OperationId: 
- Auth (Swagger): No
- Auth (Observed): Yes

#### Parameters

| Name | In | Required | Type | Description |
|---|---|---|---|---|
| id | path | Yes | integer(int32) |  |
| password | query | No | string |  |

#### Responses

| Status | Description | Content-Type | Schema |
|---|---|---|---|
| 200 | Success |  |  |

### POST /api/WebAuth/info

- OperationId: 
- Auth (Swagger): No
- Auth (Observed): Yes

#### Request Body

- Required: No

| Content-Type | Schema |
|---|---|
| application/json | WebCustomerUpdateModel |
| text/json | WebCustomerUpdateModel |
| application/*+json | WebCustomerUpdateModel |

Sample JSON request:

```json
{
    "id":  1,
    "name":  "sample",
    "avatar":  "sample",
    "phone":  "sample",
    "email":  "sample",
    "address":  "sample",
    "provinceId":  1,
    "districtId":  1,
    "wardId":  1
}
```

#### Responses

| Status | Description | Content-Type | Schema |
|---|---|---|---|
| 200 | Success |  |  |

### GET /api/WebAuth/info/{id}

- OperationId: 
- Auth (Swagger): No
- Auth (Observed): Yes

#### Parameters

| Name | In | Required | Type | Description |
|---|---|---|---|---|
| id | path | Yes | integer(int32) |  |

#### Responses

| Status | Description | Content-Type | Schema |
|---|---|---|---|
| 200 | Success |  |  |

### POST /api/WebAuth/login

- OperationId: 
- Auth (Swagger): No
- Auth (Observed): Yes

#### Request Body

- Required: No

| Content-Type | Schema |
|---|---|
| application/json | AuthenticateModel |
| text/json | AuthenticateModel |
| application/*+json | AuthenticateModel |

Sample JSON request:

```json
{
    "username":  "sample",
    "password":  "sample",
    "companyCode":  "sample"
}
```

#### Responses

| Status | Description | Content-Type | Schema |
|---|---|---|---|
| 200 | Success |  |  |

### POST /api/WebAuth/loginsocial

- OperationId: 
- Auth (Swagger): No
- Auth (Observed): No

#### Request Body

- Required: No

| Content-Type | Schema |
|---|---|
| application/json | AuthenticateSocialModel |
| text/json | AuthenticateSocialModel |
| application/*+json | AuthenticateSocialModel |

Sample JSON request:

```json
{
    "email":  "sample",
    "name":  "sample",
    "avarta":  "sample",
    "provider":  "sample",
    "photoUrl":  "sample",
    "gender":  1,
    "providerId":  "sample"
}
```

#### Responses

| Status | Description | Content-Type | Schema |
|---|---|---|---|
| 200 | Success |  |  |

### POST /api/WebAuth/register

- OperationId: 
- Auth (Swagger): No
- Auth (Observed): No

#### Request Body

- Required: No

| Content-Type | Schema |
|---|---|
| application/json | WebCustomerV2Model |
| text/json | WebCustomerV2Model |
| application/*+json | WebCustomerV2Model |

Sample JSON request:

```json
{
    "id":  1,
    "code":  "sample",
    "name":  "sample",
    "avatar":  "sample",
    "phone":  "sample",
    "email":  "sample",
    "password":  "sample",
    "address":  "sample",
    "provinceId":  1,
    "districtId":  1,
    "wardId":  1
}
```

#### Responses

| Status | Description | Content-Type | Schema |
|---|---|---|---|
| 200 | Success |  |  |

### POST /api/WebAuth/update-email

- OperationId: 
- Auth (Swagger): No
- Auth (Observed): No

#### Request Body

- Required: No

| Content-Type | Schema |
|---|---|
| application/json | WebCustomerV2Model |
| text/json | WebCustomerV2Model |
| application/*+json | WebCustomerV2Model |

Sample JSON request:

```json
{
    "id":  1,
    "code":  "sample",
    "name":  "sample",
    "avatar":  "sample",
    "phone":  "sample",
    "email":  "sample",
    "password":  "sample",
    "address":  "sample",
    "provinceId":  1,
    "districtId":  1,
    "wardId":  1
}
```

#### Responses

| Status | Description | Content-Type | Schema |
|---|---|---|---|
| 200 | Success |  |  |

## Schema Reference

### AllowanceRequest

- Type: object
- Required fields: none

| Property | Type | Description |
|---|---|---|
| amount | number(double) |  |
| code | string |  |
| description | string |  |
| name | string |  |

Sample JSON:

```json
{
    "name":  "sample",
    "code":  "sample",
    "amount":  1,
    "description":  "sample"
}
```

### AllowanceUserRequest

- Type: object
- Required fields: none

| Property | Type | Description |
|---|---|---|
| allowanceId | integer(int32) |  |
| amountOverride | number(double) |  |
| endDate | string(date-time) |  |
| note | string |  |
| startDate | string(date-time) |  |
| userId | integer(int32) |  |

Sample JSON:

```json
{
    "allowanceId":  1,
    "userId":  1,
    "startDate":  "2026-03-13T11:57:27.4226658+07:00",
    "endDate":  "2026-03-13T11:57:27.4226658+07:00",
    "amountOverride":  1,
    "note":  "sample"
}
```

### AuthenticateModel

- Type: object
- Required fields: none

| Property | Type | Description |
|---|---|---|
| companyCode | string |  |
| password | string |  |
| username | string |  |

Sample JSON:

```json
{
    "username":  "sample",
    "password":  "sample",
    "companyCode":  "sample"
}
```

### AuthenticateSocialModel

- Type: object
- Required fields: none

| Property | Type | Description |
|---|---|---|
| avarta | string |  |
| email | string |  |
| gender | integer(int32) |  |
| name | string |  |
| photoUrl | string |  |
| provider | string |  |
| providerId | string |  |

Sample JSON:

```json
{
    "email":  "sample",
    "name":  "sample",
    "avarta":  "sample",
    "provider":  "sample",
    "photoUrl":  "sample",
    "gender":  1,
    "providerId":  "sample"
}
```

### CarFieldModel

- Type: object
- Required fields: none

| Property | Type | Description |
|---|---|---|
| carId | integer(int32) |  |
| id | integer(int32) |  |
| name | string |  |
| order | integer(int32) |  |

Sample JSON:

```json
{
    "id":  1,
    "carId":  1,
    "order":  1,
    "name":  "sample"
}
```

### CarFieldSetupModel

- Type: object
- Required fields: none

| Property | Type | Description |
|---|---|---|
| carFieldId | integer(int32) |  |
| carId | integer(int32) |  |
| fileLink | string |  |
| fromAt | string(date-time) |  |
| id | integer(int32) |  |
| note | string |  |
| toAt | string(date-time) |  |
| userIdString | string |  |
| valueNumber | number(double) |  |
| warningAt | string(date-time) |  |

Sample JSON:

```json
{
    "id":  1,
    "carId":  1,
    "carFieldId":  1,
    "valueNumber":  1,
    "fromAt":  "2026-03-13T11:57:27.4423953+07:00",
    "toAt":  "2026-03-13T11:57:27.4423953+07:00",
    "warningAt":  "2026-03-13T11:57:27.4423953+07:00",
    "userIdString":  "sample",
    "note":  "sample",
    "fileLink":  "sample"
}
```

### CarLocationDetailModel

- Type: object
- Required fields: none

| Property | Type | Description |
|---|---|---|
| carLocationId | integer(int32) |  |
| driverName | string |  |
| fileStr | string |  |
| id | integer(int32) |  |
| licensePlates | string |  |
| location | string |  |
| note | string |  |
| payload | string |  |
| planExpected | string |  |
| planInprogress | string |  |
| type | string |  |

Sample JSON:

```json
{
    "id":  1,
    "carLocationId":  1,
    "licensePlates":  "sample",
    "type":  "sample",
    "payload":  "sample",
    "driverName":  "sample",
    "location":  "sample",
    "planInprogress":  "sample",
    "planExpected":  "sample",
    "note":  "sample",
    "fileStr":  "sample"
}
```

### CarLocationModel

- Type: object
- Required fields: none

| Property | Type | Description |
|---|---|---|
| date | string(date-time) |  |
| details | array<CarLocationDetailModel> |  |
| id | integer(int32) |  |
| note | string |  |
| procedureNumber | string |  |
| status | string |  |

Sample JSON:

```json
{
    "id":  1,
    "date":  "2026-03-13T11:57:27.4500037+07:00",
    "note":  "sample",
    "procedureNumber":  "sample",
    "status":  "sample",
    "details":  {
                    "id":  1,
                    "carLocationId":  1,
                    "licensePlates":  "sample",
                    "type":  "sample",
                    "payload":  "sample",
                    "driverName":  "sample",
                    "location":  "sample",
                    "planInprogress":  "sample",
                    "planExpected":  "sample",
                    "note":  "sample",
                    "fileStr":  "sample"
                }
}
```

### CarModel

- Type: object
- Required fields: none

| Property | Type | Description |
|---|---|---|
| content | string |  |
| files | array<string> |  |
| fuelAmount | number(double) |  |
| id | integer(int32) |  |
| licensePlates | string |  |
| mileageAllowance | number(double) |  |
| note | string |  |

Sample JSON:

```json
{
    "id":  1,
    "licensePlates":  "sample",
    "note":  "sample",
    "content":  "sample",
    "mileageAllowance":  1,
    "fuelAmount":  1,
    "files":  "sample"
}
```

### CertificateRequest

- Type: object
- Required fields: none

| Property | Type | Description |
|---|---|---|
| expiryDate | string(date-time) |  |
| issueDate | string(date-time) |  |
| issuer | string |  |
| name | string |  |
| note | string |  |
| userId | integer(int32) |  |

Sample JSON:

```json
{
    "userId":  1,
    "name":  "sample",
    "issuer":  "sample",
    "issueDate":  "2026-03-13T11:57:27.4601435+07:00",
    "expiryDate":  "2026-03-13T11:57:27.4601435+07:00",
    "note":  "sample"
}
```

### ChangePasswordRequest

- Type: object
- Required fields: none

| Property | Type | Description |
|---|---|---|
| id | integer(int32) |  |
| newPassword | string |  |
| oldPassword | string |  |

Sample JSON:

```json
{
    "id":  1,
    "oldPassword":  "sample",
    "newPassword":  "sample"
}
```

### ConnectPageDto

- Type: object
- Required fields: none

| Property | Type | Description |
|---|---|---|
| accessToken | string |  |
| name | string |  |
| pageId | string |  |
| userToken | string |  |

Sample JSON:

```json
{
    "pageId":  "sample",
    "name":  "sample",
    "accessToken":  "sample",
    "userToken":  "sample"
}
```

### ContractFileRequest

- Type: object
- Required fields: none

| Property | Type | Description |
|---|---|---|
| contractTypeId | integer(int32) |  |
| fileUrl | string |  |
| name | string |  |
| note | string |  |

Sample JSON:

```json
{
    "name":  "sample",
    "fileUrl":  "sample",
    "contractTypeId":  1,
    "note":  "sample"
}
```

### ContractTypeRequest

- Type: object
- Required fields: none

| Property | Type | Description |
|---|---|---|
| code | string |  |
| description | string |  |
| durationMonths | integer(int32) |  |
| name | string |  |

Sample JSON:

```json
{
    "name":  "sample",
    "code":  "sample",
    "description":  "sample",
    "durationMonths":  1
}
```

### CreatePostDto

- Type: object
- Required fields: none

| Property | Type | Description |
|---|---|---|
| aiPrompt | string |  |
| autoGenerateContent | boolean |  |
| content | string |  |
| imageUrls | string |  |
| pageId | integer(int32) |  |
| scheduledTime | string(date-time) |  |

Sample JSON:

```json
{
    "pageId":  1,
    "content":  "sample",
    "imageUrls":  "sample",
    "scheduledTime":  "2026-03-13T11:57:27.4706527+07:00",
    "autoGenerateContent":  true,
    "aiPrompt":  "sample"
}
```

### CreateUserRequest

- Type: object
- Required fields: none

| Property | Type | Description |
|---|---|---|
| address | string |  |
| birthDay | string(date-time) |  |
| companyCode | string |  |
| departmentId | integer(int32) |  |
| email | string |  |
| fullName | string |  |
| gender | integer(int32) |  |
| password | string |  |
| phone | string |  |
| positionDetailId | integer(int32) |  |
| username | string |  |
| userRoleIds | string |  |

Sample JSON:

```json
{
    "username":  "sample",
    "password":  "sample",
    "fullName":  "sample",
    "email":  "sample",
    "phone":  "sample",
    "userRoleIds":  "sample",
    "departmentId":  1,
    "positionDetailId":  1,
    "gender":  1,
    "birthDay":  "2026-03-13T11:57:27.4791530+07:00",
    "address":  "sample",
    "companyCode":  "sample"
}
```

### DecideRequest

- Type: object
- Required fields: none

| Property | Type | Description |
|---|---|---|
| decisionTypeId | integer(int32) |  |
| description | string |  |
| effectiveDate | string(date-time) |  |
| expiredDate | string(date-time) |  |
| fileUrl | string |  |
| note | string |  |
| title | string |  |
| userId | integer(int32) |  |

Sample JSON:

```json
{
    "userId":  1,
    "decisionTypeId":  1,
    "title":  "sample",
    "description":  "sample",
    "effectiveDate":  "2026-03-13T11:57:27.4821547+07:00",
    "expiredDate":  "2026-03-13T11:57:27.4841033+07:00",
    "fileUrl":  "sample",
    "note":  "sample"
}
```

### DecisionTypeRequest

- Type: object
- Required fields: none

| Property | Type | Description |
|---|---|---|
| code | string |  |
| name | string |  |
| note | string |  |

Sample JSON:

```json
{
    "name":  "sample",
    "code":  "sample",
    "note":  "sample"
}
```

### DegreeRequest

- Type: object
- Required fields: none

| Property | Type | Description |
|---|---|---|
| description | string |  |
| graduationYear | integer(int32) |  |
| name | string |  |
| school | string |  |
| userId | integer(int32) |  |

Sample JSON:

```json
{
    "userId":  1,
    "name":  "sample",
    "school":  "sample",
    "description":  "sample",
    "graduationYear":  1
}
```

### DepartmentRequest

- Type: object
- Required fields: none

| Property | Type | Description |
|---|---|---|
| code | string |  |
| name | string |  |
| order | integer(int32) |  |
| parentId | integer(int32) |  |

Sample JSON:

```json
{
    "name":  "sample",
    "code":  "sample",
    "parentId":  1,
    "order":  1
}
```

### DriverRouterModel

- Type: object
- Required fields: none

| Property | Type | Description |
|---|---|---|
| advancePaymentAmount | number(double) |  |
| amount | number(double) |  |
| date | string(date-time) |  |
| fuelAmount | number(double) |  |
| id | integer(int32) |  |
| note | string |  |
| petrolConsumptionId | integer(int32) |  |
| status | string |  |

Sample JSON:

```json
{
    "id":  1,
    "date":  "2026-03-13T11:57:27.4934226+07:00",
    "amount":  1,
    "note":  "sample",
    "status":  "sample",
    "petrolConsumptionId":  1,
    "advancePaymentAmount":  1,
    "fuelAmount":  1
}
```

### ForgotPasswordRequest

- Type: object
- Required fields: none

| Property | Type | Description |
|---|---|---|
| username | string |  |

Sample JSON:

```json
{
    "username":  "sample"
}
```

### HistoryAchievementRequest

- Type: object
- Required fields: none

| Property | Type | Description |
|---|---|---|
| achievedDate | string(date-time) |  |
| description | string |  |
| note | string |  |
| title | string |  |
| userId | integer(int32) |  |

Sample JSON:

```json
{
    "userId":  1,
    "title":  "sample",
    "description":  "sample",
    "achievedDate":  "2026-03-13T11:57:27.4979210+07:00",
    "note":  "sample"
}
```

### Inventory

- Type: object
- Required fields: none

| Property | Type | Description |
|---|---|---|
| account | string |  |
| accountName | string |  |
| closeQuantity | number(double) |  |
| closeQuantityReal | number(double) |  |
| createAt | string(date-time) |  |
| dateExpiration | string(date-time) |  |
| detail1 | string |  |
| detail2 | string |  |
| detailName1 | string |  |
| detailName2 | string |  |
| id | integer(int32) |  |
| image1 | string |  |
| inputQuantity | number(double) |  |
| isCheck | boolean |  |
| isDeleted | boolean |  |
| note | string |  |
| outputQuantity | number(double) |  |
| warehouse | string |  |
| warehouseName | string |  |

Sample JSON:

```json
{
    "id":  1,
    "account":  "sample",
    "accountName":  "sample",
    "warehouse":  "sample",
    "warehouseName":  "sample",
    "detail1":  "sample",
    "detailName1":  "sample",
    "detail2":  "sample",
    "detailName2":  "sample",
    "image1":  "sample",
    "inputQuantity":  1,
    "outputQuantity":  1,
    "closeQuantity":  1,
    "closeQuantityReal":  1,
    "createAt":  "2026-03-13T11:57:27.5016476+07:00",
    "note":  "sample",
    "dateExpiration":  "2026-03-13T11:57:27.5016476+07:00",
    "isCheck":  true,
    "isDeleted":  true
}
```

### MajorRequest

- Type: object
- Required fields: none

| Property | Type | Description |
|---|---|---|
| description | string |  |
| name | string |  |

Sample JSON:

```json
{
    "name":  "sample",
    "description":  "sample"
}
```

### Menu

- Type: object
- Required fields: code

| Property | Type | Description |
|---|---|---|
| code | string |  |
| codeParent | string |  |
| id | integer(int32) |  |
| isParent | boolean |  |
| name | string |  |
| nameEN | string |  |
| nameKO | string |  |
| note | string |  |
| order | integer(int32) |  |

Sample JSON:

```json
{
    "id":  1,
    "code":  "sample",
    "name":  "sample",
    "nameEN":  "sample",
    "nameKO":  "sample",
    "codeParent":  "sample",
    "isParent":  true,
    "order":  1,
    "note":  "sample"
}
```

### MenuRole

- Type: object
- Required fields: none

| Property | Type | Description |
|---|---|---|
| add | boolean |  |
| approve | boolean |  |
| delete | boolean |  |
| edit | boolean |  |
| id | integer(int32) |  |
| menu | Menu |  |
| menuCode | string |  |
| menuId | integer(int32) |  |
| userRole | UserRole |  |
| userRoleId | integer(int32) |  |
| view | boolean |  |

Sample JSON:

```json
{
    "id":  1,
    "menuId":  1,
    "userRoleId":  1,
    "menuCode":  "sample",
    "view":  true,
    "add":  true,
    "edit":  true,
    "delete":  true,
    "approve":  true,
    "menu":  {
                 "id":  1,
                 "code":  "sample",
                 "name":  "sample",
                 "nameEN":  "sample",
                 "nameKO":  "sample",
                 "codeParent":  "sample",
                 "isParent":  true,
                 "order":  1,
                 "note":  "sample"
             },
    "userRole":  {
                     "id":  1,
                     "code":  "sample",
                     "title":  "sample",
                     "note":  "sample",
                     "order":  1,
                     "userCreated":  1,
                     "isNotAllowDelete":  true,
                     "companyCode":  "sample",
                     "parentId":  1
                 }
}
```

### MenuViewModel

- Type: object
- Required fields: none

| Property | Type | Description |
|---|---|---|
| code | string |  |
| codeParent | string |  |
| id | integer(int32) |  |
| isParent | boolean |  |
| name | string |  |
| nameEN | string |  |
| nameKO | string |  |
| note | string |  |
| order | integer(int32) |  |

Sample JSON:

```json
{
    "id":  1,
    "code":  "sample",
    "name":  "sample",
    "nameEN":  "sample",
    "nameKO":  "sample",
    "codeParent":  "sample",
    "isParent":  true,
    "order":  1,
    "note":  "sample"
}
```

### PetrolConsumptionModel

- Type: object
- Required fields: none

| Property | Type | Description |
|---|---|---|
| advanceAmount | number(double) |  |
| carId | integer(int32) |  |
| date | string(date-time) |  |
| id | integer(int32) |  |
| kmFrom | number(double) |  |
| kmTo | number(double) |  |
| locationFrom | string |  |
| locationTo | string |  |
| note | string |  |
| petroPrice | number(double) |  |
| points | array<PetrolConsumptionPoliceCheckPointModel> |  |
| roadRouteId | integer(int32) |  |
| userId | integer(int32) |  |

Sample JSON:

```json
{
    "id":  1,
    "date":  "2026-03-13T11:57:27.5219195+07:00",
    "userId":  1,
    "carId":  1,
    "petroPrice":  1,
    "kmFrom":  1,
    "kmTo":  1,
    "locationFrom":  "sample",
    "locationTo":  "sample",
    "advanceAmount":  1,
    "note":  "sample",
    "roadRouteId":  1,
    "points":  {
                   "amount":  1,
                   "policeCheckPointName":  "sample",
                   "isArise":  true
               }
}
```

### PetrolConsumptionPoliceCheckPointModel

- Type: object
- Required fields: none

| Property | Type | Description |
|---|---|---|
| amount | number(double) |  |
| isArise | boolean |  |
| policeCheckPointName | string |  |

Sample JSON:

```json
{
    "amount":  1,
    "policeCheckPointName":  "sample",
    "isArise":  true
}
```

### PoliceCheckPointModel

- Type: object
- Required fields: none

| Property | Type | Description |
|---|---|---|
| amount | number(double) |  |
| code | string |  |
| id | integer(int32) |  |
| name | string |  |

Sample JSON:

```json
{
    "id":  1,
    "code":  "sample",
    "name":  "sample",
    "amount":  1
}
```

### PositionDetailRequest

- Type: object
- Required fields: none

| Property | Type | Description |
|---|---|---|
| code | string |  |
| departmentId | integer(int32) |  |
| name | string |  |
| note | string |  |
| order | integer(int32) |  |

Sample JSON:

```json
{
    "name":  "sample",
    "code":  "sample",
    "departmentId":  1,
    "note":  "sample",
    "order":  1
}
```

### RegisterRequest

- Type: object
- Required fields: none

| Property | Type | Description |
|---|---|---|
| companyCode | string |  |
| email | string |  |
| fullName | string |  |
| password | string |  |
| username | string |  |
| userRoleIds | string |  |

Sample JSON:

```json
{
    "username":  "sample",
    "password":  "sample",
    "fullName":  "sample",
    "email":  "sample",
    "userRoleIds":  "sample",
    "companyCode":  "sample"
}
```

### RelativeRequest

- Type: object
- Required fields: none

| Property | Type | Description |
|---|---|---|
| address | string |  |
| name | string |  |
| note | string |  |
| phone | string |  |
| relationship | string |  |
| userId | integer(int32) |  |

Sample JSON:

```json
{
    "userId":  1,
    "name":  "sample",
    "relationship":  "sample",
    "phone":  "sample",
    "address":  "sample",
    "note":  "sample"
}
```

### RoadRouteModel

- Type: object
- Required fields: none

| Property | Type | Description |
|---|---|---|
| code | string |  |
| id | integer(int32) |  |
| name | string |  |
| numberOfTrips | number(double) |  |
| policeCheckPointIdStr | string |  |
| roadRouteDetail | string |  |

Sample JSON:

```json
{
    "id":  1,
    "code":  "sample",
    "name":  "sample",
    "roadRouteDetail":  "sample",
    "policeCheckPointIdStr":  "sample",
    "numberOfTrips":  1
}
```

### SalaryTypeRequest

- Type: object
- Required fields: none

| Property | Type | Description |
|---|---|---|
| baseAmount | number(double) |  |
| code | string |  |
| formula | string |  |
| name | string |  |
| note | string |  |

Sample JSON:

```json
{
    "name":  "sample",
    "code":  "sample",
    "baseAmount":  1,
    "formula":  "sample",
    "note":  "sample"
}
```

### TimeKeepingEntryRequest

- Type: object
- Required fields: none

| Property | Type | Description |
|---|---|---|
| checkIn | string(date-time) |  |
| checkOut | string(date-time) |  |
| note | string |  |
| userId | integer(int32) |  |
| workDate | string(date-time) |  |
| workingHours | number(double) |  |

Sample JSON:

```json
{
    "userId":  1,
    "workDate":  "2026-03-13T11:57:27.5429795+07:00",
    "checkIn":  "2026-03-13T11:57:27.5429795+07:00",
    "checkOut":  "2026-03-13T11:57:27.5429795+07:00",
    "workingHours":  1,
    "note":  "sample"
}
```

### UserContractHistoryRequest

- Type: object
- Required fields: none

| Property | Type | Description |
|---|---|---|
| contractTypeId | integer(int32) |  |
| endDate | string(date-time) |  |
| fileUrl | string |  |
| note | string |  |
| signedDate | string(date-time) |  |
| startDate | string(date-time) |  |
| userId | integer(int32) |  |

Sample JSON:

```json
{
    "userId":  1,
    "contractTypeId":  1,
    "signedDate":  "2026-03-13T11:57:27.5459806+07:00",
    "startDate":  "2026-03-13T11:57:27.5469704+07:00",
    "endDate":  "2026-03-13T11:57:27.5469704+07:00",
    "fileUrl":  "sample",
    "note":  "sample"
}
```

### UserMenuAssignDto

- Type: object
- Required fields: none

| Property | Type | Description |
|---|---|---|
| add | boolean |  |
| delete | boolean |  |
| edit | boolean |  |
| menuCode | string |  |
| menuId | integer(int32) |  |
| view | boolean |  |

Sample JSON:

```json
{
    "menuId":  1,
    "menuCode":  "sample",
    "view":  true,
    "add":  true,
    "edit":  true,
    "delete":  true
}
```

### UserRole

- Type: object
- Required fields: code

| Property | Type | Description |
|---|---|---|
| code | string |  |
| companyCode | string |  |
| id | integer(int32) |  |
| isNotAllowDelete | boolean |  |
| note | string |  |
| order | integer(int32) |  |
| parentId | integer(int32) |  |
| title | string |  |
| userCreated | integer(int32) |  |

Sample JSON:

```json
{
    "id":  1,
    "code":  "sample",
    "title":  "sample",
    "note":  "sample",
    "order":  1,
    "userCreated":  1,
    "isNotAllowDelete":  true,
    "companyCode":  "sample",
    "parentId":  1
}
```

### WarehouseFloorSetterModel

- Type: object
- Required fields: none

| Property | Type | Description |
|---|---|---|
| code | string |  |
| id | integer(int32) |  |
| name | string |  |
| note | string |  |
| positionIds | array<integer(int32)> |  |

Sample JSON:

```json
{
    "id":  1,
    "code":  "sample",
    "name":  "sample",
    "note":  "sample",
    "positionIds":  1
}
```

### WareHousePosition

- Type: object
- Required fields: none

| Property | Type | Description |
|---|---|---|
| code | string |  |
| createdDate | string(date-time) |  |
| id | integer(int32) |  |
| isDeleted | boolean |  |
| name | string |  |
| note | string |  |
| updatedDate | string(date-time) |  |
| userCreated | integer(int32) |  |
| userUpdated | integer(int32) |  |

Sample JSON:

```json
{
    "id":  1,
    "code":  "sample",
    "name":  "sample",
    "note":  "sample",
    "createdDate":  "2026-03-13T11:57:27.5526175+07:00",
    "updatedDate":  "2026-03-13T11:57:27.5526175+07:00",
    "userCreated":  1,
    "userUpdated":  1,
    "isDeleted":  true
}
```

### WarehouseSetterModel

- Type: object
- Required fields: none

| Property | Type | Description |
|---|---|---|
| branchId | integer(int32) |  |
| code | string |  |
| id | integer(int32) |  |
| isSyncChartOfAccount | boolean |  |
| managerName | string |  |
| name | string |  |
| shelveIds | array<integer(int32)> |  |

Sample JSON:

```json
{
    "id":  1,
    "branchId":  1,
    "name":  "sample",
    "code":  "sample",
    "managerName":  "sample",
    "isSyncChartOfAccount":  true,
    "shelveIds":  1
}
```

### WarehouseShelvesSetterModel

- Type: object
- Required fields: none

| Property | Type | Description |
|---|---|---|
| code | string |  |
| floorIds | array<integer(int32)> |  |
| id | integer(int32) |  |
| name | string |  |
| note | string |  |
| orderHorizontal | integer(int32) |  |
| orderVertical | integer(int32) |  |

Sample JSON:

```json
{
    "id":  1,
    "code":  "sample",
    "name":  "sample",
    "note":  "sample",
    "orderHorizontal":  1,
    "orderVertical":  1,
    "floorIds":  1
}
```

### WebCustomerUpdateModel

- Type: object
- Required fields: none

| Property | Type | Description |
|---|---|---|
| address | string |  |
| avatar | string |  |
| districtId | integer(int32) |  |
| email | string |  |
| id | integer(int32) |  |
| name | string |  |
| phone | string |  |
| provinceId | integer(int32) |  |
| wardId | integer(int32) |  |

Sample JSON:

```json
{
    "id":  1,
    "name":  "sample",
    "avatar":  "sample",
    "phone":  "sample",
    "email":  "sample",
    "address":  "sample",
    "provinceId":  1,
    "districtId":  1,
    "wardId":  1
}
```

### WebCustomerV2Model

- Type: object
- Required fields: none

| Property | Type | Description |
|---|---|---|
| address | string |  |
| avatar | string |  |
| code | string |  |
| districtId | integer(int32) |  |
| email | string |  |
| id | integer(int32) |  |
| name | string |  |
| password | string |  |
| phone | string |  |
| provinceId | integer(int32) |  |
| wardId | integer(int32) |  |

Sample JSON:

```json
{
    "id":  1,
    "code":  "sample",
    "name":  "sample",
    "avatar":  "sample",
    "phone":  "sample",
    "email":  "sample",
    "password":  "sample",
    "address":  "sample",
    "provinceId":  1,
    "districtId":  1,
    "wardId":  1
}
```

### WorkflowDto

- Type: object
- Required fields: none

| Property | Type | Description |
|---|---|---|
| name | string |  |
| triggerType | string |  |
| workflowJson | string |  |

Sample JSON:

```json
{
    "name":  "sample",
    "workflowJson":  "sample",
    "triggerType":  "sample"
}
```

