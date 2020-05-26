context('SideMenu', () => {
    beforeEach(() => {
        cy.visit('http://localhost:8080');
    });

    it('visit inventory page via logo', () => {
        cy.get('#inventoryTitle').contains('Inventory Dashboard');
    });

    it('has buttons to add and receiver shipments', () => {
        cy.get('#addNewBtn > .solar-button').contains('Add New Item');
        cy.get('#receiveShipmentBtn > .solar-button').contains('Receive Shipment');
    });

    it('has add new item modal on click', () => {
        cy.get('#addNewBtn > .solar-button').click();
        cy.get('#modalTitle').contains('Add New Product')
        cy.get('[aria-label="close modal"] > .solar-button').click();
    });

    it('adding new product and close modal before saving the product', () => {
        cy.get('#addNewBtn > .solar-button').click();
        cy.get('#isTaxable').click();
        cy.get('#productName').clear();
        cy.get('#productName').type('Cypress Test', {delay: 80});
        cy.get('#productDesc').clear();
        cy.get('#productDesc').type('A great new test product for sale', {delay: 60});
        cy.get('#productPrice').clear();
        cy.get('#productPrice').type('1200', {delay: 50});
        cy.get('[aria-label="close modal"] > .solar-button').click();
    });

    it('adding new product and save', () => {
        cy.get('#addNewBtn > .solar-button').click();
        cy.get('#isTaxable').click();
        cy.get('#productName').clear();
        cy.get('#productName').type('Cypress Test', {delay: 80});
        cy.get('#productDesc').clear();
        cy.get('#productDesc').type('A great new test product for sale', {delay: 60});
        cy.get('#productPrice').clear();
        cy.get('#productPrice').type('1200', {delay: 50});
        cy.get('[aria-label="save new item"] > .solar-button').click();
        cy.get('table').contains('td', 'Cypress Test');
    });

    it('archive a product', () => {
        cy.get('#addNewBtn > .solar-button').click();
        cy.get('#isTaxable').click();
        cy.get('#productName').clear();
        cy.get('#productName').type('Cypress Test', {delay: 80});
        cy.get('#productDesc').clear();
        cy.get('#productDesc').type('A great new test product for sale', {delay: 60});
        cy.get('#productPrice').clear();
        cy.get('#productPrice').type('1200', {delay: 50});
        cy.get('[aria-label="save new item"] > .solar-button').click();
        cy.get('#inventoryTable > tr > td > div').last().click();
    });
});
