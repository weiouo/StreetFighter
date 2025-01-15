/*
 * File:   test.c
 * Author: Wei Yu
 *
 * Created on 2024?12?13?, ?? 3:21
 */
#include <xc.h>
#include <pic18f4520.h>
#include <stdio.h>

#pragma config OSC = INTIO67 // Oscillator Selection bits
#pragma config WDT = OFF     // Watchdog Timer Enable bit
#pragma config PWRT = OFF    // Power-up Enable bit
#pragma config BOREN = ON    // Brown-out Reset Enable bit
#pragma config PBADEN = OFF  // Watchdog Timer Enable bit
#pragma config LVP = OFF     // Low Voltage (single -supply) In-Circute Serial Pragramming Enable bit
#pragma config CPD = OFF     // Data EEPROM?Memory Code Protection bit (Data EEPROM code protection off)

int ADC_Read(int channel)
{
    int digital;
    
    ADCON0bits.CHS =  0x00; // Select Channe7
    ADCON0bits.GO = 1;
    ADCON0bits.ADON = 1;
    
    while(ADCON0bits.GO_nDONE==1);

    digital = (ADRESH*256) | (ADRESL);
    return(digital);
}

void UART_Initialize() {         
    
    //  Setting baud rate
    TXSTAbits.SYNC = 0;           
    BAUDCONbits.BRG16 = 0;          
    TXSTAbits.BRGH = 0;      
    
   //   Serial enable                               
    PIE1bits.TXIE = 0;    //interrupt
    IPR1bits.TXIP = 0;    //high priority / low priority      
    PIE1bits.RCIE = 0;    //interrupt          
    IPR1bits.RCIP = 0;    //high priority / low priority    
    
   }
void UART_Write(unsigned char data)  // Output on Terminal
{
    while(!TXSTAbits.TRMT);
    TXREG = data;              //write to TXREG will send data 
}

void main() {
    
    TRISA = 0xff;		// Set as input port
    ADCON1 = 0x0e;  	// Ref vtg is VDD & Configure pin as analog pin   	
    ADFM = 1 ;          // Right Justifie
    ADCON2bits.ADCS = 7; // 
    ADRESH = 0;  			// Flush ADC output Register
    ADRESL = 0;
    TRISB = 3;
    PORTB = 0;
    
    TRISC = 0;
    INTCONbits.INT0IF = 0;
    INTCON3bits.INT1IF = 0;
    INTCONbits.GIE = 1;
    INTCONbits.INT0IE = 1;
    INTCON3bits.INT1IP = 1;
    INTCON3bits.INT1IE = 1;
    
    TXSTAbits.SYNC = 0;   // Set UART to asynchronous mode
    RCSTAbits.SPEN = 1;   // Enable serial port (TX/RX)
    TXSTAbits.TXEN = 1;   // Enable UART transmitter
    RCSTAbits.CREN = 1;   // Enable UART receiver
    SPBRG = 12;        // Set baud rate to 1200 (for 1 MHz clock)
    
    UART_Initialize();
    unsigned int x ;
    
    while (1){
        x = ADC_Read(0);
        if(x<256){
            UART_Write('l');
        }
        else if (x<768){
            UART_Write('n');
        }
        else{
            UART_Write('r');
        }  
    }
}

void __interrupt(high_priority) Hi_ISR(void){
    if (INT0IF){
        UART_Write('A');
        for(int i=0;i<100;i++){
            for (int j=0;j<50;j++);
        }
        INT0IF = 0;
    }
    if (INT1IF){
        UART_Write('B');
        for(int i=0;i<100;i++){
            for (int j=0;j<50;j++);
        }
        INT1IF = 0;
    }
}
