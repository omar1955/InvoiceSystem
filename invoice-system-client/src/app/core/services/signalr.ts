import { Injectable } from '@angular/core';
import * as signalR from '@microsoft/signalr';

import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class SignalrService {
  private connection?: signalR.HubConnection;

  startConnection(): Promise<void> {
    if (this.connection?.state === signalR.HubConnectionState.Connected) {
      return Promise.resolve();
    }

    this.connection = new signalR.HubConnectionBuilder()
      .withUrl(environment.hubUrl)
      .withAutomaticReconnect()
      .build();

    return this.connection
      .start()
      .then(() => {
        console.log('SignalR connected');
      })
      .catch(error => {
        console.error('SignalR connection error:', error);
      });
  }

  onInvoicesUpdated(callback: () => void): void {
    if (!this.connection) {
      return;
    }

    this.connection.off('InvoicesUpdated');

    this.connection.on('InvoicesUpdated', (data) => {
      console.log('InvoicesUpdated received:', data);
      callback();
    });
  }

  stopConnection(): void {
    this.connection?.stop();
    this.connection = undefined;
  }
}