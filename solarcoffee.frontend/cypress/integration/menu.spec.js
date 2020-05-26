const { createYield } = require("typescript");

context('SideMenu', () => {
    beforeEach(() => {
        cy.visit('http://localhost:8080');
    });

    it('visit inventory page via logo', () => {
        cy.get('#imageLogo').click();
        cy.get('#inventoryTitle').contains('Inventory Dashboard');
    });

    it('visit the inventory page via button', () => {
        cy.get('#menuInventory').click();
        cy.get('#inventoryTitle').contains('Inventory Dashboard');
    });

    it('visit the customer page via button', () => {
        cy.get('#menuCustomer').click();
        cy.get('#customersTitle').contains('Customer');
    });

    it('visit the invoice page via button', () => {
        cy.get('#menuInvoice').click();
        cy.get('#invoiceTitle').contains('Invoice');
    });

    it('visit the orders page via button', () => {
        cy.get('#menuOrders').click();
        cy.get('#orderTitle').contains('Orders');
    });
});
