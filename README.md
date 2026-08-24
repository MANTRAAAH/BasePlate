🍕 BasePlate - Turnkey Digital Ecosystem & SaaS
===============================================

**Settore:** Food & Beverage / Hospitality (PMI, Ristoranti, B&B, Dark Kitchens, Franchising)**Modello di Business:** White-Label B2B SaaS Multi-Tenant**Stato Sicurezza & Compliance:** GDPR Compliant, Data Isolation by Design 🟢 _(Architettura completata)_

BasePlate non è un semplice gestionale per la ristorazione. È il primo **Ecosistema Operativo Ibrido** progettato per risolvere contemporaneamente l'acquisizione clienti (SEO Locale) e l'efficienza in sala, abbattendo gli attriti operativi. Sviluppato da chi conosce realmente i ritmi della sala e del bancone, offre un'interfaccia zero-friction per lo staff e un motore ad alte prestazioni per la vetrina pubblica.

🚀 Architettura IT & Cloud Infrastructure (Dual-Engine)
-------------------------------------------------------

L'infrastruttura sfrutta un paradigma a microservizi logici su un database condiviso, garantendo prestazioni edge, sicurezza enterprise e costi infrastrutturali marginali vicini allo zero per ogni nuovo tenant.

### Backend & Database (Master Hub) 🟢 _(Fase 1 Completata)_

*   **Core API (.NET 9 / C#):** Architettura RESTful e asincrona. Gestisce l'orchestrazione, la validazione delle logiche di business e il routing dei Webhook.
    
*   **Database (PostgreSQL 16+):** Modello _Shared Schema, Multi-Tenant_. Isolamento garantito tramite filtri nativi di Entity Framework Core (IMustHaveTenant e Global Query Filters). L'ID del ristorante è estratto dinamicamente dal token JWT o tramite header.
    
*   **Data Flexibility:** Utilizzo di campi JSONB per storicizzare preferenze grafiche e configurazioni specifiche dei tenant senza alterare lo schema DDL.
    
*   **Real-Time & Caching:** Hub WebSocket bidirezionale tramite **SignalR** per latenza zero tra Sala e Cucina, supportato da **Redis** (LRU cache) per servire il catalogo menu.
    

### Frontend Dual-Engine

*   **Engine 1 - Gestionale Interno (Angular + Tailwind):** SPA reattiva per le rotte operative (/admin, /sala, /kds). Potenziata con _Service Workers_ e _IndexedDB_ per Offline Tolerance durante le micro-disconnessioni.
    
*   **Engine 2 - Vetrina Pubblica (Astro + Tailwind):** Framework SSG/Hybrid déployato su infrastruttura Edge. Garantisce tempi di risposta in millisecondi (TTFB) e punteggi Lighthouse perfetti.
    

📦 Core Modules & Stato di Sviluppo (Roadmap)
---------------------------------------------

### 1\. SuperAdmin & Master Provisioning 🟡 _(Fase 2 - In Chiusura)_

Il centro di comando per gestire il parco clienti SaaS, con un'attenzione maniacale alla sicurezza.

*   **Multi-Tenant Orchestrator:** Pannello di telemetria, statistiche in tempo reale e routing protetto da Guard.
    
*   **God Mode & Impersonation \[🟢 Fatto\]:** Accesso diretto agli ambienti dei tenant senza richiedere le password (tramite X-Tenant-Id).
    
*   **Sicurezza Dati \[🟢 Fatto\]:** Eliminazione Protetta con "Double-Check" visivo e distruzione SQL a cascata per prevenire record orfani.
    
*   **Onboarding One-Click (Wizard) \[🚧 Da Fare\]:** Automazione per nuovi clienti divisa in 3 step (Identità & Sede, Accesso, Configurazione Ecosistema). Generazione tenant, iniezione dati master e provisioning.
    
*   **Motore Routing Domini \[🚧 Da Fare\]:** Gestione sottodomini nativi (\*.baseplate.app) e switch per domini personalizzati White-Label.
    
*   **Billing Integrato:** Collegamento nativo con Stripe Connect per abbonamenti SaaS.
    

### 2\. Master Data Management (PIM) 🟡 _(Fase 3 - In Corso)_

Il cuore pulsante del database del ristorante.

*   **Gestore Catalogo Multidimensionale \[🟢 DB Fatto, 🚧 UI Da Fare\]:** Database relazionale per gestire prodotti, varianti, formati, impasti speciali e supplementi con pricing dinamico.
    
*   **Motore Allergeni Propagativo \[🚧 Da Fare\]:**
    
*   Il database relazionale è strutturato correttamente (mappatura molti-a-molti), ma manca l'implementazione della logica a eventi nel backend C#. Bisogna sviluppare il
    
*   _trigger_
    
*   che, all'aggiunta di un allergene su un ingrediente, aggiorni a cascata la presenza di quell'allergene su tutte le pietanze correlate.
    
*   **RBAC (Role-Based Access Control) \[🟢 Fatto\]:** Viste isolate in base al ruolo (Store Manager, Cuoco, Cameriere).
    

### 3\. Vetrina e Conversione SEO (Astro) ⚪ _(Fase 4 - Da Iniziare)_

*   **Edge SEO:** Pre-rendering HTML per un layout statico e velocissimo.
    
*   **Schema.org e JSON-LD Dinamico:** Iniezione profonda di microdati. Prodotti e recensioni diventano entità scansionabili nativamente da Google.
    
*   **Active Call Staff:** Sistema di notifiche push per la chiamata al tavolo (da tenere in cantiere per le evoluzioni future).
    

### 4\. Operatività di Sala & KDS (Angular PWA) ⚪ _(Fase 4 - Da Iniziare)_

Progettato per abbattere gli errori sotto stress con un'ergonomia studiata sul campo.

*   **Spatial Mapping 2D:** Mappatura visuale Drag&Drop dei tavoli.
    
*   **Ergonomia Estrema (BYOD):** Dark Mode obbligatoria e target tattili maggiorati per evitare tap accidentali.
    
*   **Algoritmo Vettoriale "Split-Conto":** UI visuale a "vassoi" per trascinare singoli item sui pagatori.
    
*   **KDS (Kitchen Display System):** Monitor suddivisi per partita connessi via SignalR con timer cromatici.
    

### 5\. Hardware Integration & Fiscal Local Bridge ⚪ _(Fase 5 - Da Iniziare)_

*   **Worker Asincrono .NET 9 (Local Bridge):** Demone installato localmente che funge da proxy per bypassare i limiti del browser web.
    
*   **Teleassistenza & Logging:** Telemetria remota via **Serilog** per interventi silenti senza recarsi sul posto.
    
*   **Integrazione Fiscale Standard:** Utilizzo di SDK nativi (Custom, Epson) per comandare cassetti e Registratori Telematici con retry policy avanzate.
    

📈 Financial Model & Go-To-Market
---------------------------------

BasePlate trasforma una spesa passiva (le commissioni del delivery) in un investimento ad alto rendimento, offrendo ai ristoratori il controllo totale sui propri dati.

*   **Setup Iniziale (Una Tantum):** Copre l'infrastruttura locale (Hardware Bridge), il data-entry del menu, il tuning SEO e la formazione.
    
*   **Licenza SaaS Premium (Ricorrente Annuale):** Garantisce hosting ad alte prestazioni, SLA del 99.9%, failover automatici e mantenimento dell'infrastruttura vetrina.
    
*   **Upselling:** Moduli di food costing, API headless per terze parti e integrazioni con aggregatori di delivery.
