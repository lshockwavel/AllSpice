CREATE TABLE IF NOT EXISTS accounts(
  id VARCHAR(255) NOT NULL PRIMARY KEY COMMENT 'primary key',
  created_at DATETIME DEFAULT CURRENT_TIMESTAMP COMMENT 'Time Created',
  updated_at DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP COMMENT 'Last Update',
  name VARCHAR(255) COMMENT 'User Name',
  email VARCHAR(255) UNIQUE COMMENT 'User Email',
  picture VARCHAR(255) COMMENT 'User Picture'
) default charset utf8mb4 COMMENT '';



CREATE TABLE recipe(
  id INT NOT NULL PRIMARY KEY AUTO_INCREMENT COMMENT 'primary key',
  created_at DATETIME DEFAULT CURRENT_TIMESTAMP COMMENT 'Time Created',
  updated_at DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP COMMENT 'Last Update',
  title VARCHAR(255) NOT NULL COMMENT 'Recipe Title',
  category ENUM ('breakfast', 'lunch', 'dinner', 'dessert', 'snack') NOT NULL COMMENT 'Recipe Category',
  instructions VARCHAR(5000) COMMENT 'Recipe Instructions',
  img VARCHAR(1000) NOT NULL COMMENT 'Image URL',
  creator_id VARCHAR(255) NOT NULL COMMENT 'Creator ID From Accounts Table',
  FOREIGN KEY (creator_id) REFERENCES accounts(id) ON DELETE CASCADE
)

CREATE TABLE ingredients(
  id INT NOT NULL PRIMARY KEY AUTO_INCREMENT COMMENT 'primary key',
  created_at DATETIME DEFAULT CURRENT_TIMESTAMP COMMENT 'Time Created',
  updated_at DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP COMMENT 'Last Update',
  name VARCHAR(255) NOT NULL COMMENT 'Ingredient Name',
  quantity VARCHAR(100) COMMENT 'Ingredient Quantity',
  recipe_id INT NOT NULL COMMENT 'Recipe ID From Recipe Table',
  FOREIGN KEY (recipe_id) REFERENCES recipe(id) ON DELETE CASCADE
)

CREATE TABLE favorites(
  id INT NOT NULL PRIMARY KEY AUTO_INCREMENT COMMENT 'primary key',
  created_at DATETIME DEFAULT CURRENT_TIMESTAMP COMMENT 'Time Created',
  updated_at DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP COMMENT 'Last Update',
  recipe_id INT NOT NULL COMMENT 'Recipe ID From Recipe Table',
  account_id VARCHAR(255) NOT NULL COMMENT 'Account ID From Accounts Table',
  FOREIGN KEY (recipe_id) REFERENCES recipe(id) ON DELETE CASCADE,
  FOREIGN KEY (account_id) REFERENCES accounts(id) ON DELETE CASCADE
);


SELECT favorites.*, recipe.*, accounts.*
        FROM favorites
        INNER JOIN recipe ON favorites.recipe_id = recipe.id
        INNER JOIN accounts ON recipe.creator_id = accounts.id
        WHERE favorites.id = 1;


SELECT favorites.*, recipe.*
        FROM favorites
        INNER JOIN recipe ON favorites.recipe_id = recipe.id
        WHERE favorites.id = 1;

        
        INSERT INTO recipe
        (title, instructions, img, category, creator_id)
        VALUES
        ("Spaghetti Bolognese", "1. Cook spaghetti according to package instructions.\n2. In a separate pan, sauté onions and garlic until translucent.\n3. Add ground beef and cook until browned.\n4. Stir in tomato sauce and let simmer for 15 minutes.\n5. Serve sauce over spaghetti and garnish with Parmesan cheese.", "https://images.unsplash.com/photo-1622973536968-3ead9e780960?q=80&w=1740&auto=format&fit=crop&ixlib=rb-4.1.0&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D", "dinner", "691b95d9426378972ae47eb6");
        SELECT LAST_INSERT_ID();
        
        SELECT recipe.*, accounts.*
        FROM recipe
        INNER JOIN accounts ON recipe.creator_id = accounts.id
        WHERE recipe.id = 1;