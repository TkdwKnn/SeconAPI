CREATE DATABASE secon_db;

\c secon_db;

-- Create users table
CREATE TABLE IF NOT EXISTS users (
    id SERIAL PRIMARY KEY,
    username VARCHAR(50) NOT NULL UNIQUE,
    email VARCHAR(100) NOT NULL UNIQUE,
    password VARCHAR(255) NOT NULL,
    password_salt VARCHAR(255) NOT NULL,
    role VARCHAR(20) NOT NULL DEFAULT 'User',
    created_at TIMESTAMP NOT NULL DEFAULT NOW(),
    last_login TIMESTAMP NULL
);

-- Create user_tokens table
CREATE TABLE IF NOT EXISTS user_tokens (
    id SERIAL PRIMARY KEY,
    user_id INTEGER NOT NULL REFERENCES users(id) ON DELETE CASCADE,
    token VARCHAR(255) NOT NULL UNIQUE,
    created_at TIMESTAMP NOT NULL DEFAULT NOW(),
    expires_at TIMESTAMP NOT NULL,
    is_revoked BOOLEAN NOT NULL DEFAULT FALSE
);

-- Create documents table
CREATE TABLE IF NOT EXISTS documents (
    id SERIAL PRIMARY KEY,
    user_id INTEGER NOT NULL REFERENCES users(id) ON DELETE CASCADE,
    date TIMESTAMP NOT NULL DEFAULT NOW(),
    fields JSONB NOT NULL,
    file_name VARCHAR(255) NULL,
    content_type VARCHAR(100) NULL,
    status VARCHAR(20) NOT NULL DEFAULT 'Pending'
);

-- Create indexes
CREATE INDEX idx_users_username ON users(username);
CREATE INDEX idx_user_tokens_token ON user_tokens(token);
CREATE INDEX idx_user_tokens_user_id ON user_tokens(user_id);
CREATE INDEX idx_documents_user_id ON documents(user_id);

-- Insert admin user (password: admin123)
INSERT INTO users (username, email, password, password_salt, role)
VALUES (
    'admin',
    'admin@example.com',
    'T/PdKzOoJtIrGUG5TT7yMrkqGT3ddv9vlfJa6FHX4ZM=', -- hashed 'admin123' with salt
    'ZJqQnRVDlfRQJFKlLGnMOYWYsnXYbWYAHoUxjwTwjaI=', -- sample salt
    'Admin'
);
