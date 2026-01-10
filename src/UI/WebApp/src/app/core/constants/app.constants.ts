export const API_ENDPOINTS = {
  USERS: '/api/users',
  AUTH: '/api/auth'
};

export const HTTP_STATUS = {
  OK: 200,
  CREATED: 201,
  NO_CONTENT: 204,
  BAD_REQUEST: 400,
  UNAUTHORIZED: 401,
  FORBIDDEN: 403,
  NOT_FOUND: 404,
  CONFLICT: 409,
  INTERNAL_SERVER_ERROR: 500
};

export const VALIDATION_MESSAGES = {
  REQUIRED: 'This field is required',
  EMAIL: 'Please enter a valid email address',
  MIN_LENGTH: (length: number) => `Minimum length is ${length} characters`,
  MAX_LENGTH: (length: number) => `Maximum length is ${length} characters`,
  PASSWORD_PATTERN: 'Password must contain uppercase, lowercase, and numbers'
};

export const NOTIFICATION_DURATION = {
  SHORT: 2000,
  MEDIUM: 3000,
  LONG: 5000
};
