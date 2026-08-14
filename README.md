# 🚀 EXECUTIVE PROJECT BLUEPRINT (EXTENDED)
**Progetto:** BasePlate - Turnkey Digital Ecosystem & SaaS (Smart Table, MDM, Web)  
**Settore:** Food & Beverage / Hospitality (PMI, Ristoranti, B&B, Dark Kitchens, Franchising)  
**Modello di Business:** White-Label B2B SaaS Multi-Tenant  
**Stato Sicurezza & Compliance:** GDPR Compliant, Data Isolation by Design

---

## 📊 1. Modello Finanziario, Scalabilità e Go-to-Market
L'ingegnerizzazione del software si basa su un'architettura Multi-Tenant che abbatte drasticamente i costi marginali per ogni nuovo cliente acquisito. Il sistema è concepito per generare MRR (Monthly Recurring Revenue) ad alto margine, trattenendo il cliente tramite un lock-in tecnologico basato sull'estrema efficienza operativa.

### Struttura dei Ricavi (Unit Economics)
*   **Setup e Onboarding Iniziale (Una Tantum: € 2.000 - € 3.500):**
    *   Provisioning del dominio personalizzato e configurazione DNS.
    *   Configurazione del Theme Engine (Palette, Loghi, Tipografia) per il sito vetrina.
    *   Mappatura spaziale 2D della sala e setup dell'infrastruttura di rete/KDS in loco.
    *   Formazione dello staff (abbattuta grazie alla UX zero-friction).
*   **Licenza SaaS Premium (Ricorrente: € 1.500 - € 2.400/anno):**
    *   Include hosting cloud, failover, backup giornalieri, CDN globale per la vetrina e aggiornamenti continui.
    *   SLA (Service Level Agreement) garantito al 99.9% di uptime per il servizio in sala.
*   **Upselling e Moduli Aggiuntivi (Revenue Expansion):**
    *   Integrazioni con delivery terzi (Glovo, Deliveroo, UberEats).
    *   Accesso Headless API per agenzie che sviluppano front-end custom per il ristoratore.
    *   Moduli avanzati di Food Costing e gestione magazzino integrata.

### Strategia di Acquisizione (GTM)
Il posizionamento sul mercato è netto: BasePlate non è solo un "gestionale per scontrini", ma il primo **Ecosistema Operativo Ibrido**. Risolve il problema del marketing (attrazione clienti tramite SEO locale a punteggio 100/100) e il problema operativo (turnover tavoli, errori di comanda e logoramento dello staff).

---

## ⚙️ 2. Architettura IT & Cloud Infrastructure (Dual-Engine)
L'ecosistema sfrutta un paradigma a microservizi logici su un database condiviso, garantendo prestazioni edge e sicurezza enterprise.

### Backend & Database (Master Hub)
*   **Core API (.NET 9 / C#):** Architettura RESTful e asincrona. Gestisce l'orchestrazione, la validazione delle logiche di business e il routing dei Webhook.
*   **Database (PostgreSQL 16+):** Modello *Shared Schema, Multi-Tenant*. Utilizzo estensivo di indici B-Tree e campi `JSONB` per storicizzare le preferenze grafiche o le variazioni di configurazione specifiche di ogni Ristorante senza alterare lo schema DDL.
*   **Isolamento (Filtri EF Core):** Intercettazione a basso livello tramite l'interfaccia `IMustHaveTenant` e i **Global Query Filters**. Il `RistoranteId` viene estratto dal Claim del JWT ad ogni singola richiesta HTTP. Il backend è matematicamente impossibilitato a "sporcare" i dati tra tenant diversi.
*   **Real-Time & Caching:** 
    *   **SignalR:** Hub WebSocket per la comunicazione bidirezionale a bassissima latenza (Sala <-> Cucina).
    *   **Redis:** Layer di caching in-memory con policy LRU (Least Recently Used) per servire il catalogo menu senza colpire il DB relazionale durante i picchi del sabato sera.

### Frontend Dual-Engine
*   **Engine 1 - Gestionale Interno (Angular + Tailwind):** SPA protetta e reattiva, potenziata con i *Signals* per la gestione dello stato. Rendering CSR per le rotte operative (`/admin`, `/sala`, `/kds`). Implementazione di *Service Workers* e *IndexedDB* per gestire micro-disconnessioni di rete in sala senza perdere la comanda in corso (Offline Tolerance).
*   **Engine 2 - Vetrina Pubblica (Astro + Tailwind):** Framework SSG/Hybrid déployato su infrastruttura Edge (es. Cloudflare Pages o Vercel). Pagine pre-generate in HTML puro al momento del salvataggio dei dati dall'Admin, minimizzando il Time To First Byte (TTFB) a pochi millisecondi.

---

## 📦 3. Work Breakdown Structure (Moduli di Sistema Dettagliati)

### Modulo A: SuperAdmin & Master Provisioning
*   **Multi-Tenant Orchestrator:** Pannello di controllo esclusivo per il gestore della piattaforma (Tu). Fornisce telemetria sull'utilizzo delle risorse CPU/RAM per singolo Tenant.
*   **Onboarding One-Click:** Automazione transazionale. Alla pressione del tasto, il backend:
    1. Registra il Ristorante e l'Utente Admin.
    2. Inietta un set di dati Master (es. aliquote IVA, categorie standard).
    3. Lancia una chiamata API alla CDN per generare il certificato SSL del nuovo dominio.
    4. Spara un Webhook all'Engine Astro per triggerare la prima build statica del nuovo sito.
*   **Billing Integrato:** Collegamento nativo con Stripe Connect per la fatturazione automatica mensile o annuale dei canoni SaaS ai ristoratori.

### Modulo B: Vetrina e Conversione SEO (Astro)
*   **Edge SEO e Core Web Vitals:** Grazie al rendering statico di Astro, il layout (Liquid Glass, Sharp Pastel, ecc.) viene risolto a tempo di build. Risultato: punteggi di performance costantemente sul 98-100% per Google Lighthouse, dominando la SERP locale.
*   **Schema.org e JSON-LD Dinamico:** Iniezione profonda di microdati per `Restaurant`, `Menu`, `MenuItem` e `Offer`. Le pizze, i prezzi e le recensioni diventano entità che Google legge e mostra direttamente nelle schede di ricerca.
*   **Smart Wishlist & Carrello Ibrido:** Il cliente scansiona il QR code al tavolo. Il sistema rileva tramite parametro URL (es. `?tavolo=12`) la posizione esatta. Può compilare l'ordine autonomamente e inviarlo (innescando un ping in sala per la validazione del cameriere) o usarlo solo come catalogo interattivo.

### Modulo C: Operatività di Sala & KDS (Angular PWA)
L'interfaccia è frutto dell'analisi sul campo: progettata per operatori sotto stress e in movimento costante.
*   **Canvas Editor & Spatial Mapping:** Il ristoratore mappa la sala con uno strumento visivo Drag&Drop. Il cameriere non guarda una sterile lista di numeri, ma la rappresentazione topografica del locale, riducendo a zero gli errori di consegna.
*   **Ergonomia Estrema (BYOD - Bring Your Own Device):** UI tassativamente in *Dark Mode* per azzerare l'affaticamento visivo e consumare meno batteria sugli schermi OLED degli smartphone personali. Target tattili (bottoni) maggiorati per evitare miss-click.
*   **Algoritmo Vettoriale "Split-Conto":** Risoluzione del peggior collo di bottiglia in cassa. UI visuale a "vassoi". Si trascinano i singoli dettagli dell'ordine sui vari pagatori. Il sistema calcola dinamicamente le frazioni (es. bottiglie d'acqua e coperti divisi automaticamente per N persone), generando scontrini parziali o ricevute telematiche perfette al centesimo.
*   **KDS (Kitchen Display System):** Monitor rugged in cucina collegati via SignalR. Le comande esplodono a schermo per partita (Forno, Friggitoria, Cucina). Timer cromatici indicano i ritardi (es. scontrino che passa al rosso dopo 15 minuti di attesa).

### Modulo D: Master Data Management (PIM)
*   **Gestore Catalogo Multidimensionale:** Non un semplice listino, ma un database relazionale di prodotti, varianti (es. formati "Normale", "Maxi"), impasti (es. "Senza Glutine", "Pinsa") e supplementi, tutti con regole rigide di pricing (sovrapprezzo percentuale o fisso).
*   **Motore Allergeni Propagativo:** Collegamento molti-a-molti tra `Ingrediente` e `Allergene`. Se il ristoratore marca la "Farina" con glutine, tutte le pizze che la contengono si aggiornano in automatico in tempo reale su menu e palmari, tutelando legalmente l'attività.
*   **Gestione Permessi Granulare (RBAC):** Role-Based Access Control. Il ristoratore crea account limitati: il "Cameriere" vede solo la mappa e le comande; il "Cuoco" vede solo il KDS; lo "Store Manager" vede reportistica e statistiche.

### Modulo E: Integrazioni Fiscali e Periferiche
*   **Worker Asincrono .NET:** Servizio in background che gestisce le code di stampa. Se la rete Wi-Fi locale salta un istante, il worker mette il task in coda e riprova (Retry Policy), assicurando che nessuna comanda vada persa nel tragitto Sala-Cucina.
*   **Adapter di Stampa ed EPSON/ESC-POS:** Moduli di traduzione dati per comunicare direttamente con le stampanti di reparto LAN o con i registratori telematici cloud-based, estromettendo l'hardware proprietario obsoleto.
